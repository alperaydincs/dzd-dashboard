using DZDDashboard.Common.Constants;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DZDDashboard.Services;

public partial class UserService
{
    public async Task<int> SyncEntraUserAsync(string objectId, string? email, string? firstName, string? lastName, bool hasElevatedRole = false, CancellationToken cancellationToken = default)
    {
        var existing = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.EntraObjectId == objectId, cancellationToken);

        if (existing is not null) return existing.Id;

        var normalizedEmail = string.IsNullOrWhiteSpace(email) ? null : email.ToUpperInvariant();

        if (normalizedEmail is not null)
        {
            var byEmail = await context.Users
                .FirstOrDefaultAsync(u => u.EntraObjectId == null && u.NormalizedEmail == normalizedEmail, cancellationToken);

            if (byEmail is not null)
            {
                byEmail.EntraObjectId = objectId;
                if (string.IsNullOrWhiteSpace(byEmail.FirstName)) byEmail.FirstName = firstName;
                if (string.IsNullOrWhiteSpace(byEmail.LastName))  byEmail.LastName  = lastName;
                await context.SaveChangesAsync(cancellationToken);
                return byEmail.Id;
            }
        }

        var newUser = new User
        {
            EntraObjectId   = objectId,
            Email           = email,
            NormalizedEmail = normalizedEmail,
            FirstName       = firstName,
            LastName        = lastName,
            Slug            = await GenerateUniqueSlugAsync(email, firstName, lastName, cancellationToken),
            IsActive        = true,
            LifecycleStatus = UserLifecycleStatuses.Active
        };

        context.Users.Add(newUser);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return newUser.Id;
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            logger.LogWarning(
                "Race condition in SyncEntraUserAsync: concurrent insert for EntraObjectId {ObjectId}. Fetching winning row.",
                objectId);
            context.ChangeTracker.Clear();
            var winningId = await context.Users
                .AsNoTracking()
                .Where(u => u.EntraObjectId == objectId)
                .Select(u => (int?)u.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (winningId is null)
                throw new InvalidOperationException($"Concurrent SyncEntraUserAsync: row for EntraObjectId '{objectId}' not found after unique-constraint race.");
            return winningId.Value;
        }
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        => ex.InnerException is Npgsql.PostgresException pgEx
           && pgEx.SqlState == Npgsql.PostgresErrorCodes.UniqueViolation;
}
