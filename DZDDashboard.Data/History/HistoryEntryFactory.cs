using System.Collections.Concurrent;
using System.Reflection;
using DZDDashboard.Data.Abstractions;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DZDDashboard.Data.History;

/// <summary>
/// Builds a typed "*History" snapshot row for any EntityWithHistory change, purely from EF's
/// change-tracking metadata. Adding history support for a new entity requires no code here -
/// only a "{Entity}History" POCO (in the DZDDashboard.Data.Entities.History namespace) whose
/// property names match the ones to be mirrored. Properties left off the History POCO are
/// simply not copied (e.g. StoredFile.Content is intentionally omitted).
/// </summary>
public static class HistoryEntryFactory
{
    private static readonly ConcurrentDictionary<Type, HistoryMapping?> Mappings = new();

    public static IHistoryEntity? CreateSnapshot(EntityEntry<EntityWithHistory> entry, DateTime recordedAt, int? recordedById)
    {
        var operation = entry.State switch
        {
            EntityState.Added => HistoryOperation.Insert,
            EntityState.Modified => HistoryOperation.Update,
            EntityState.Deleted => HistoryOperation.Delete,
            _ => (HistoryOperation?)null
        };
        if (operation is null) return null;

        var mapping = Mappings.GetOrAdd(entry.Entity.GetType(), _ => BuildMapping(entry));
        if (mapping is null) return null;

        var useOriginalValues = entry.State != EntityState.Added;
        var history = (IHistoryEntity)Activator.CreateInstance(mapping.HistoryType)!;

        history.Operation = operation.Value;
        history.HistoryRecordedAt = recordedAt;
        history.HistoryRecordedById = recordedById;

        foreach (var (efProperty, historyProperty) in mapping.DirectProperties)
        {
            var value = useOriginalValues ? entry.OriginalValues[efProperty] : entry.CurrentValues[efProperty];
            historyProperty.SetValue(history, value);
        }

        foreach (var owned in mapping.OwnedNavigations)
        {
            var targetEntry = entry.Reference(owned.NavigationName).TargetEntry;
            if (targetEntry is null) continue;

            foreach (var (efProperty, historyProperty) in owned.Properties)
            {
                var value = useOriginalValues ? targetEntry.OriginalValues[efProperty] : targetEntry.CurrentValues[efProperty];
                historyProperty.SetValue(history, value);
            }
        }

        return history;
    }

    private static HistoryMapping? BuildMapping(EntityEntry<EntityWithHistory> entry)
    {
        var entityType = entry.Entity.GetType();
        var historyType = entityType.Assembly.GetType(
            $"{typeof(HistoryOperation).Namespace}.{entityType.Name}History");

        if (historyType is null || !typeof(IHistoryEntity).IsAssignableFrom(historyType))
            return null;

        var historyProps = historyType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(p => p.Name, p => p);

        var directProperties = new List<(IProperty, PropertyInfo)>();
        foreach (var efProperty in entry.Metadata.GetProperties())
        {
            if (historyProps.TryGetValue(efProperty.Name, out var historyProperty))
                directProperties.Add((efProperty, historyProperty));
        }

        var ownedNavigations = new List<OwnedMapping>();
        foreach (var navigation in entry.Metadata.GetNavigations().Where(n => !n.IsCollection && n.TargetEntityType.IsOwned()))
        {
            var ownedProperties = new List<(IProperty, PropertyInfo)>();
            foreach (var efProperty in navigation.TargetEntityType.GetProperties())
            {
                if (historyProps.TryGetValue(navigation.Name + efProperty.Name, out var historyProperty))
                    ownedProperties.Add((efProperty, historyProperty));
            }

            if (ownedProperties.Count > 0)
                ownedNavigations.Add(new OwnedMapping(navigation.Name, ownedProperties));
        }

        return new HistoryMapping(historyType, directProperties, ownedNavigations);
    }

    private sealed record HistoryMapping(
        Type HistoryType,
        List<(IProperty EfProperty, PropertyInfo HistoryProperty)> DirectProperties,
        List<OwnedMapping> OwnedNavigations);

    private sealed record OwnedMapping(
        string NavigationName,
        List<(IProperty EfProperty, PropertyInfo HistoryProperty)> Properties);
}
