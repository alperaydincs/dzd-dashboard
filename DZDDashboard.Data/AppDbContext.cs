using DZDDashboard.Data.Abstractions;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
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
    public DbSet<EducationHistory> EducationHistories { get; set; }
    public DbSet<PositionHistory> PositionHistories { get; set; }
    public DbSet<StoredFile> Files { get; set; }
    public DbSet<SalaryHistory> SalaryHistories { get; set; }
    public DbSet<BenefitRecord> BenefitRecords { get; set; }
    public DbSet<BenefitDependent> BenefitDependents { get; set; }
    public DbSet<AdditionalPayment> AdditionalPayments { get; set; }
    public DbSet<Deduction> Deductions { get; set; }
    public DbSet<OrganizationPosition> OrganizationPositions { get; set; }
    public DbSet<OnboardingProcess> OnboardingProcesses { get; set; }
    public DbSet<OffboardingProcess> OffboardingProcesses { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<ProcessTemplate> ProcessTemplates { get; set; }
    public DbSet<ChecklistStepTemplate> ChecklistStepTemplates { get; set; }
    public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
    public DbSet<ProcessDocument> ProcessDocuments { get; set; }
    public DbSet<LifecycleAuditLogEntry> LifecycleAuditLogEntries { get; set; }
    public DbSet<UdemyCourseActivity> UdemyCourseActivities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

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

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = now;

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedAt = now;
                if (currentUserId.HasValue)
                    entry.Entity.ModifiedById = currentUserId.Value;
            }
        }
    }
}
