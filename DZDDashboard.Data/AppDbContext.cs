using DZDDashboard.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace DZDDashboard.Data;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor? httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<ProjectDocument> ProjectDocuments { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<WorkType> WorkTypes { get; set; }
    public DbSet<CareerMapRule> CareerMapRules { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<DefaultDocument> DefaultDocuments { get; set; }
    public DbSet<DzdStatus> DzdStatuses { get; set; }
    public DbSet<ExCompanyHistory> ExCompanyHistories { get; set; }
    public DbSet<HeadLeadCoefficient> HeadLeadCoefficients { get; set; }
    public DbSet<IssuePriority> IssuePriorities { get; set; }
    public DbSet<IssueStatus> IssueStatuses { get; set; }
    public DbSet<IssueType> IssueTypes { get; set; }
    public DbSet<Itsm> Itsms { get; set; }
    public DbSet<IssuePaymentType> IssuePaymentTypes { get; set; }
    public DbSet<JiraStatus> JiraStatuses { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<PayrollLocation> PayrollLocations { get; set; }
    public DbSet<Period> Periods { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectBonusCoefficient> ProjectBonusCoefficients { get; set; }
    public DbSet<ProjectInvoice> ProjectInvoices { get; set; }
    public DbSet<Resolution> Resolutions { get; set; }
    public DbSet<Salesforce> SalesForces { get; set; }
    public DbSet<TargetEffort> TargetEfforts { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<UserDocumentCategory> UserDocumentCategories { get; set; }
    public DbSet<UserDocument> UserDocuments { get; set; }
    public DbSet<UserAvatar> UserAvatars { get; set; }
    public DbSet<ChildInfo> ChildInfos { get; set; }
    public DbSet<EmergencyContact> EmergencyContacts { get; set; }
    public DbSet<EducationHistory> EducationHistories { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<OrganizationPosition> OrganizationPositions { get; set; }

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
        if (_httpContextAccessor?.HttpContext == null) return;

        var now = DateTime.UtcNow;
        var currentUserId = ResolveCurrentUserId(_httpContextAccessor.HttpContext.User);

        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedAt = now;
                if (currentUserId.HasValue)
                    entry.Entity.ModifiedById = currentUserId.Value;
            }
        }
    }

    private static int? ResolveCurrentUserId(ClaimsPrincipal? user)
    {
        if (user == null) return null;

        var databaseUserId = user.FindFirst("database_user_id")?.Value;
        if (int.TryParse(databaseUserId, out var parsedDatabaseUserId))
        {
            return parsedDatabaseUserId;
        }

        var nameIdentifier = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(nameIdentifier, out var parsedNameIdentifier))
        {
            return parsedNameIdentifier;
        }

        return null;
    }
}