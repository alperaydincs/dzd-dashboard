using DZDDashboard.Data.Abstractions;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Entities.History;
using DZDDashboard.Data.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace DZDDashboard.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, IAuditProvider? auditProvider = null)
    : DbContext(options)
{
    private readonly IAuditProvider _audit = auditProvider ?? NullAuditProvider.Instance;

    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CareerPath> CareerPaths { get; set; }
    public DbSet<CareerPathRule> CareerPathRules { get; set; }
    public DbSet<CareerPathRuleJob> CareerPathRuleJobs { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<PayrollLocation> PayrollLocations { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<UserCvDocument> UserCvDocuments { get; set; }
    public DbSet<ChildInfo> ChildInfos { get; set; }
    public DbSet<EmergencyContact> EmergencyContacts { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<StoredFile> Files { get; set; }
    public DbSet<Salary> Salaries { get; set; }
    public DbSet<BenefitPayment> BenefitPayments { get; set; }
    public DbSet<BenefitPaymentDependent> BenefitPaymentDependents { get; set; }
    public DbSet<AdditionalPayment> AdditionalPayments { get; set; }
    public DbSet<Deduction> Deductions { get; set; }
    public DbSet<OrganizationPosition> OrganizationPositions { get; set; }
    public DbSet<UdemyCourseActivity> UdemyCourseActivities { get; set; }

    public DbSet<UserHistory> UserHistories { get; set; }
    public DbSet<DepartmentHistory> DepartmentHistories { get; set; }
    public DbSet<CompanyHistory> CompanyHistories { get; set; }
    public DbSet<TeamHistory> TeamHistories { get; set; }
    public DbSet<JobHistory> JobHistories { get; set; }
    public DbSet<PayrollLocationHistory> PayrollLocationHistories { get; set; }
    public DbSet<OrganizationPositionHistory> OrganizationPositionHistories { get; set; }
    public DbSet<CareerPathHistory> CareerPathHistories { get; set; }
    public DbSet<CareerPathRuleHistory> CareerPathRuleHistories { get; set; }
    public DbSet<StoredFileHistory> StoredFileHistories { get; set; }
    public DbSet<UserCvDocumentHistory> UserCvDocumentHistories { get; set; }
    public DbSet<EmergencyContactHistory> EmergencyContactHistories { get; set; }
    public DbSet<EducationHistory> EducationHistories { get; set; }
    public DbSet<ChildInfoHistory> ChildInfoHistories { get; set; }
    public DbSet<PositionHistory> PositionHistories { get; set; }
    public DbSet<SalaryHistory> SalaryHistories { get; set; }
    public DbSet<HealthInsuranceBenefitHistory> HealthInsuranceBenefitHistories { get; set; }
    public DbSet<PensionBenefitHistory> PensionBenefitHistories { get; set; }
    public DbSet<OtherBenefitHistory> OtherBenefitHistories { get; set; }
    public DbSet<BenefitPaymentDependentHistory> BenefitPaymentDependentHistories { get; set; }
    public DbSet<AdditionalPaymentHistory> AdditionalPaymentHistories { get; set; }
    public DbSet<DeductionHistory> DeductionHistories { get; set; }
    public DbSet<UdemyCourseActivityHistory> UdemyCourseActivityHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<NullableUtcDateTimeConverter>();
    }

    private sealed class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
        v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

    private sealed class NullableUtcDateTimeConverter() : ValueConverter<DateTime?, DateTime?>(
        v => v == null ? null : v.Value.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc),
        v => v == null ? null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var now           = _audit.GetNow();
        var currentUserId = _audit.GetCurrentUserId();

        // ToList(): we Add() history snapshots below, which would otherwise mutate
        // the ChangeTracker's entry collection while this loop is enumerating it.
        foreach (var entry in ChangeTracker.Entries<EntityWithHistory>().ToList())
        {
            var snapshot = HistoryEntryFactory.CreateSnapshot(entry, now, currentUserId);
            if (snapshot is not null)
                Add(snapshot);
        }
    }
}
