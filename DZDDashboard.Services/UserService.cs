using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

// Interface is in Abstractions/IUserService.cs

public class UserService(
    IMapper mapper,
    AppDbContext context,
    IReportsToCalculator reportsToCalculator,
    ILogger<UserService> logger)
    : IUserService, IUserReadService, IUserWriteService, IUserSyncService
{
    public async Task UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        if (dto.PayrollLocationId.HasValue &&
            !await context.PayrollLocations.AnyAsync(p => p.Id == dto.PayrollLocationId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(PayrollLocation), dto.PayrollLocationId.Value);

        user.FirstName           = dto.FirstName;
        user.LastName            = dto.LastName;
        user.RegistrationNumber  = dto.RegistrationNumber;
        user.UserStartDate       = dto.UserStartDate;
        user.PositionStartDate   = dto.PositionStartDate;
        user.ContractType        = dto.ContractType;
        user.ContractEndDate     = dto.ContractEndDate;
        user.WorkModel           = dto.WorkModel;
        user.PayrollLocationId   = dto.PayrollLocationId;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateContactsAsync(int userId, UpdateContactsDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        user.Email               = dto.Email;
        user.NormalizedEmail     = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.ToUpperInvariant();
        user.PhoneNumber         = dto.PhoneNumber;
        user.PersonalEmail       = dto.PersonalEmail;
        user.PersonalPhoneNumber = dto.PersonalPhoneNumber;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCitizenshipInfoAsync(int userId, UpdateCitizenshipInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        user.DateOfBirth       = dto.DateOfBirth;
        user.Gender            = dto.Gender;
        user.Nationality       = dto.Nationality;
        user.CitizenshipNumber = dto.CitizenshipNumber;
        user.DisabilityStatus  = dto.DisabilityStatus;
        user.DisabilityDegree  = dto.DisabilityDegree;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAddressInfoAsync(int userId, UpdateAddressInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        user.LegalAddress   = dto.LegalAddress;
        user.CurrentAddress = dto.CurrentAddress;
        user.City           = dto.City;
        user.Country        = dto.Country;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateEducationInfoAsync(int userId, UpdateEducationInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserWithAsync(userId, u => u.EducationHistories!, cancellationToken);

        if (user.EducationHistories?.Count > 0)
            context.EducationHistories.RemoveRange(user.EducationHistories);

        user.EducationHistories = (dto.EducationHistories ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.Level) && !string.IsNullOrWhiteSpace(x.Institution))
            .Select(x => new EducationHistory
            {
                UserId          = userId,
                Level           = x.Level,
                Institution     = x.Institution,
                Program         = x.Program,
                GraduationDate  = x.GraduationDate,
                Status          = x.Status
            })
            .ToList();

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateMyContactInfoAsync(int userId, UpdateContactInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);
        user.PhoneNumber         = dto.WorkPhoneNumber;
        user.PersonalEmail       = dto.PersonalEmail;
        user.PersonalPhoneNumber = dto.PersonalPhoneNumber;
        // Email (work email) is intentionally not updated here — it is managed by Entra ID sync
        // and can only be changed by an Admin via UpdateContactsAsync.
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateEmergencyContactsAsync(int userId, UpdateEmergencyContactsDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserWithAsync(userId, u => u.EmergencyContacts!, cancellationToken);
        user.EmergencyContacts ??= [];

        MergeCollection(
            existing:     user.EmergencyContacts,
            incoming:     dto.EmergencyContacts,
            isValid:      c => !string.IsNullOrWhiteSpace(c.FullName)
                            && !string.IsNullOrWhiteSpace(c.PhoneNumber)
                            && !string.IsNullOrWhiteSpace(c.Relationship),
            getDtoId:     c => c.Id,
            getEntityId:  e => e.Id,
            removeEntity: e => context.EmergencyContacts.Remove(e),
            createEntity: c => { var e = mapper.Map<EmergencyContact>(c); e.UserId = user.Id; return e; });

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateFamilyInfoAsync(int userId, UpdateFamilyInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserWithAsync(userId, u => u.Children!, cancellationToken);

        user.MaritalStatus  = dto.MaritalStatus;
        user.SpouseFullName = dto.SpouseFullName;
        user.Children     ??= [];

        MergeCollection(
            existing:     user.Children,
            incoming:     dto.Children,
            isValid:      c => !string.IsNullOrWhiteSpace(c.FullName) && c.DateOfBirth != null,
            getDtoId:     c => c.Id,
            getEntityId:  e => e.Id,
            removeEntity: e => context.ChildInfos.Remove(e),
            createEntity: c => { var e = mapper.Map<ChildInfo>(c); e.UserId = user.Id; return e; });

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserProfileDto?> GetProfileByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<UserProfileDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);


    public async Task<PagedResult<UserSummaryDto>> GetAllSummariesAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        // Active-only by default — deactivated employees should not appear in employee lists.
        // If admin needs to see inactive users, add an optional bool includeInactive param.
        // Trade-off: CountAsync + query is 2 DB round-trips. Acceptable here as Users table is small.
        // AsNoTracking on baseQuery so both CountAsync and the items query skip the tracking overhead.
        var baseQuery = context.Users.AsNoTracking().Where(u => u.IsActive);
        var total = await baseQuery.CountAsync(cancellationToken);
        var items = await baseQuery
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(u => new UserSummaryDto
            {
                Id              = u.Id,
                FirstName       = u.FirstName,
                LastName        = u.LastName,
                Email           = u.Email,
                PhoneNumber     = u.PhoneNumber,
                City            = u.City,
                Country         = u.Country,
                IsActive        = u.IsActive,
                UserStartDate   = u.UserStartDate,
                DepartmentId           = u.DepartmentId,
                OrganizationPositionId = u.OrganizationPositionId,
                // Projection: only ContentType loaded — ContentBase64 intentionally excluded
                Avatar = u.Avatar == null ? null : new UserAvatarSummaryDto
                {
                    Id          = u.Avatar.Id,
                    ContentType = u.Avatar.ContentType
                },
                Department = u.Department == null ? null : new DepartmentDto
                {
                    Id        = u.Department.Id,
                    Name      = u.Department.Name,
                    CompanyId = u.Department.CompanyId
                },
                Team = u.Team == null ? null : new TeamDto
                {
                    Id           = u.Team.Id,
                    Name         = u.Team.Name,
                    DepartmentId = u.Team.DepartmentId
                },
                Job = u.Job == null ? null : new JobDto
                {
                    Id   = u.Job.Id,
                    Name = u.Job.Title
                }
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<UserSummaryDto>
        {
            Items      = items,
            TotalCount = total,
            Page       = page,
            PageSize   = pageSize
        };
    }

    /// <summary>
    /// Returns the employee card DTO using <c>ProjectTo</c> so EF Core generates a SELECT
    /// that only fetches columns referenced by <see cref="EmployeeCardDto"/> — avoids loading
    /// all 40+ User columns and replaces the 12-Include chain with a single projected query.
    /// PII fields (DateOfBirth, Citizenship, Family, Address) are intentionally excluded via
    /// AutoMapper <c>Ignore()</c> and must be fetched separately via <see cref="GetSensitiveInfoAsync"/>.
    /// </summary>
    public async Task<EmployeeCardDto?> GetEmployeeCardAsync(int id, CancellationToken cancellationToken = default)
        => await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<EmployeeCardDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<UserAvatarDto?> GetAvatarByUserIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.UserAvatars
            .AsNoTracking()
            .Where(a => a.UserId == id)
            .ProjectTo<UserAvatarDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<EmployeeSensitiveInfoDto?> GetSensitiveInfoAsync(int id, CancellationToken cancellationToken = default)
    {
        // Projection: only PII columns + Children are selected — avoids loading 40+ irrelevant columns.
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new EmployeeSensitiveInfoDto
            {
                UserId              = u.Id,
                DateOfBirth         = u.DateOfBirth,
                Gender              = u.Gender,
                Nationality         = u.Nationality,
                CitizenshipNumber   = u.CitizenshipNumber,
                DisabilityStatus    = u.DisabilityStatus,
                DisabilityDegree    = u.DisabilityDegree,
                MaritalStatus       = u.MaritalStatus,
                SpouseFullName      = u.SpouseFullName,
                PersonalEmail       = u.PersonalEmail,
                PersonalPhoneNumber = u.PersonalPhoneNumber,
                LegalAddress        = u.LegalAddress,
                CurrentAddress      = u.CurrentAddress,
                City                = u.City,
                Country             = u.Country,
                Children            = u.Children!.Select(c => new ChildInfoDto
                {
                    Id          = c.Id,
                    FullName    = c.FullName,
                    DateOfBirth = c.DateOfBirth
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> SyncEntraUserAsync(string objectId, string? email, string? firstName, string? lastName, CancellationToken cancellationToken = default)
    {
        var existing = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.EntraObjectId == objectId, cancellationToken);

        if (existing is not null) return existing.Id;

        var newUser = new User
        {
            EntraObjectId   = objectId,
            Email           = email,
            NormalizedEmail = string.IsNullOrWhiteSpace(email) ? null : email.ToUpperInvariant(),
            FirstName       = firstName,
            LastName        = lastName,
            IsActive        = true
        };

        context.Users.Add(newUser);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return newUser.Id;
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            // Race condition: another request inserted the same EntraObjectId concurrently.
            // The unique index on EntraObjectId prevents duplicates — fetch the winner's row.
            logger.LogWarning(
                "Race condition in SyncEntraUserAsync: concurrent insert for EntraObjectId {ObjectId}. Fetching winning row.",
                objectId);
            // ChangeTracker.Clear() is safer than Entry(..).State = Detached:
            // it releases all tracked entities so the subsequent read cannot be
            // confused with any pending add/modify state.
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

    /// <summary>
    /// Returns true when the <see cref="DbUpdateException"/> was caused by a unique-index
    /// violation (SQL Server error 2627 — primary key / 2601 — unique index).
    /// Prevents the race-condition fallback from masking unrelated DB errors.
    /// </summary>
    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        => ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx
           && (sqlEx.Number == 2627 || sqlEx.Number == 2601);

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        // Salary/grade history is audit data — Restrict delete to prevent loss (see UserConfiguration).
        // TargetEfforts, UserTrainings, ExCompanyHistories use Cascade (operational data, deleted with user).
        // Short-circuit: skip the second query when the first already confirms conflict.
        if (await context.SalaryHistories.AnyAsync(h => h.UserId == id, cancellationToken)
            || await context.GradeHistories.AnyAsync(h => h.UserId == id, cancellationToken))
            throw new DomainConflictException(
                "Cannot delete a user with salary or grade history. Archive the history records first.");

        var user = await context.Users.FindRequiredAsync(id, nameof(User), cancellationToken);
        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAvatarAsync(int userId, string contentType, string base64Content, CancellationToken cancellationToken = default)
    {
        // Base64 padding adds ~33% overhead. Check corresponding byte size.
        var maxBytes = AvatarConstants.MaxFileSizeBytes;
        var approxBytes = base64Content.Length * 3 / 4;
        if (approxBytes > maxBytes) throw new DomainValidationException($"File size exceeds {maxBytes / 1024 / 1024} MB limit.");

        var user = await context.Users
            .Include(u => u.Avatar)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new EntityNotFoundException("User", userId);

        user.Avatar             ??= new UserAvatar { UserId = userId };
        user.Avatar.ContentType   = contentType;
        user.Avatar.ContentBase64 = base64Content;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateOrganizationPositionAsync(int userId, int? organizationPositionId, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        if (organizationPositionId.HasValue)
        {
            var exists = await context.OrganizationPositions.AnyAsync(p => p.Id == organizationPositionId.Value, cancellationToken);
            if (!exists) throw new EntityNotFoundException("OrganizationPosition", organizationPositionId.Value);
        }

        // Transaction: the position change and the recalculation must be atomic.
        // If RecalculateAsync fails, the user's OrganizationPositionId is rolled back.
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        user.OrganizationPositionId = organizationPositionId;
        user.ReportsToId            = null;
        // Flush the position change to DB first so RecalculateAsync reads the updated state.
        // RecalculateAsync uses AsNoTracking queries — it reads from DB, not the EF tracker.
        await context.SaveChangesAsync(cancellationToken);
        await reportsToCalculator.RecalculateAsync(cancellationToken); // uses ExecuteUpdateAsync within the tx
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateCareerAssignmentAsync(int userId, UpdateCareerAssignmentDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        // FK existence checks are sequential — EF Core DbContext is not thread-safe and must not be
        // used concurrently (Task.WhenAll on the same context is undefined behaviour).
        // Each check is a single-column indexed lookup and is fast in practice.
        if (dto.DepartmentId.HasValue && !await context.Departments.AnyAsync(d => d.Id == dto.DepartmentId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(Department), dto.DepartmentId.Value);

        if (dto.TeamId.HasValue && !await context.Teams.AnyAsync(t => t.Id == dto.TeamId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(Team), dto.TeamId.Value);

        if (dto.JobId.HasValue && !await context.Jobs.AnyAsync(j => j.Id == dto.JobId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(Job), dto.JobId.Value);

        if (dto.CareerPathId.HasValue && !await context.CareerPaths.AnyAsync(p => p.Id == dto.CareerPathId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(CareerPath), dto.CareerPathId.Value);

        user.CompanyName  = dto.CompanyName;
        user.DepartmentId = dto.DepartmentId;
        user.TeamId       = dto.TeamId;
        user.CareerPathId = dto.CareerPathId;
        user.JobId        = dto.JobId;
        user.Grade        = dto.Grade;

        await context.SaveChangesAsync(cancellationToken);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private async Task<User> RequireUserAsync(int userId, CancellationToken cancellationToken = default)
        => await context.Users.FindAsync([userId], cancellationToken)
           ?? throw new EntityNotFoundException("User", userId);

    /// <summary>
    /// Merges an incoming DTO list into a tracked entity collection:
    /// removes entities absent from the incoming list, updates matches, inserts new ones.
    /// Eliminates the duplicate add/remove loop in UpdateEmergencyContactsAsync and UpdateFamilyInfoAsync.
    /// </summary>
    private void MergeCollection<TEntity, TDto>(
        ICollection<TEntity> existing,
        IEnumerable<TDto>    incoming,
        Func<TDto,    bool>    isValid,
        Func<TDto,    int>     getDtoId,
        Func<TEntity, int>     getEntityId,
        Action<TEntity>        removeEntity,
        Func<TDto, TEntity>    createEntity)
        where TEntity : class
    {
        var valid   = incoming.Where(isValid).ToList();
        var keepIds = valid.Select(getDtoId).Where(id => id > 0).ToHashSet();

        foreach (var entity in existing.Where(e => !keepIds.Contains(getEntityId(e))).ToList())
            removeEntity(entity);

        foreach (var dto in valid)
        {
            var id     = getDtoId(dto);
            var match  = id > 0 ? existing.FirstOrDefault(e => getEntityId(e) == id) : null;

            if (match != null) mapper.Map(dto, match);
            else               existing.Add(createEntity(dto));
        }
    }

    /// <summary>
    /// Loads a user with a navigation property included.
    /// Use this instead of duplicating the Include+FirstOrDefault+throw pattern.
    /// </summary>
    private async Task<User> RequireUserWithAsync(int userId,
        System.Linq.Expressions.Expression<Func<User, object?>> include,
        CancellationToken cancellationToken = default)
        => await context.Users
               .Include(include)
               .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
           ?? throw new EntityNotFoundException("User", userId);

}
