using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;


public class UserService(
    IMapper mapper,
    AppDbContext context,
    IReportsToCalculator reportsToCalculator,
    IOptions<OnboardingOptions> onboardingOptions,
    ILogger<UserService> logger)
    : IUserService, IUserReadService, IUserWriteService, IUserSyncService
{
    public async Task UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        if (dto.PayrollLocationId.HasValue &&
            !await context.PayrollLocations.AnyAsync(p => p.Id == dto.PayrollLocationId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(PayrollLocation), dto.PayrollLocationId.Value);

        var nameChanged = user.FirstName != dto.FirstName || user.LastName != dto.LastName;

        user.FirstName           = dto.FirstName;
        user.LastName            = dto.LastName;
        user.RegistrationNumber  = dto.RegistrationNumber;
        user.UserStartDate       = dto.UserStartDate;
        user.PositionStartDate   = dto.PositionStartDate;
        user.ContractTypeId      = await ResolveTypeIdAsync(context.ContractTypes, dto.ContractType, cancellationToken);
        user.ContractEndDate     = dto.ContractEndDate;
        user.WorkModelId         = await ResolveTypeIdAsync(context.WorkModels, dto.WorkModel, cancellationToken);
        user.PayrollLocationId   = dto.PayrollLocationId;

        if (nameChanged)
            user.Slug = await GenerateUniqueSlugAsync(dto.FirstName, dto.LastName, cancellationToken);

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

        user.LegalAddress        = dto.LegalAddress;
        user.LegalAddressCity    = dto.LegalAddressCity;
        user.LegalAddressCountry = dto.LegalAddressCountry;
        user.CurrentAddress      = dto.CurrentAddress;
        user.City                = dto.City;
        user.Country             = dto.Country;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateEducationInfoAsync(int userId, UpdateEducationInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserWithAsync(userId, u => u.EducationHistories!, cancellationToken);

        if (user.EducationHistories?.Count > 0)
            context.EducationHistories.RemoveRange(user.EducationHistories);

        var levelMap = await context.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(l => l.Name, l => l.Id, cancellationToken);

        user.EducationHistories = (dto.EducationHistories ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.Level) && !string.IsNullOrWhiteSpace(x.Institution))
            .Select(x => new EducationHistory
            {
                UserId          = userId,
                EducationLevelId = x.Level is not null && levelMap.TryGetValue(x.Level, out var lid) ? lid : null,
                Institution     = x.Institution,
                Program         = x.Program,
                GraduationDate  = x.GraduationDate,
                Status          = x.Status
            })
            .ToList();

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePositionHistoryAsync(int userId, UpdatePositionHistoryDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        if (dto.DepartmentId.HasValue && !await context.Departments.AnyAsync(d => d.Id == dto.DepartmentId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(Department), dto.DepartmentId.Value);
        if (dto.TeamId.HasValue && !await context.Teams.AnyAsync(t => t.Id == dto.TeamId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(Team), dto.TeamId.Value);

        var current = await context.PositionHistories
            .Where(p => p.UserId == userId && p.EndDate == null)
            .OrderByDescending(p => p.StartDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (current is null)
        {
            var jobTitle = user.JobId == null ? null
                : await context.Jobs.Where(j => j.Id == user.JobId).Select(j => j.Title).FirstOrDefaultAsync(cancellationToken);
            current = new PositionHistory { UserId = userId, JobTitle = jobTitle, Grade = user.Grade };
            context.PositionHistories.Add(current);
        }

        current.CompanyName  = dto.CompanyName;
        current.DepartmentId = dto.DepartmentId;
        current.TeamId       = dto.TeamId;
        current.StartDate    = dto.StartDate;
        current.EndDate      = dto.EndDate;

        user.CompanyName  = dto.CompanyName;
        user.DepartmentId = dto.DepartmentId;
        user.TeamId       = dto.TeamId;

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SnapshotPositionIfChangedAsync(User user, int? prevGrade, int? prevJobId, CancellationToken cancellationToken)
    {
        var gradeChanged = prevGrade != user.Grade;
        var jobChanged   = prevJobId != user.JobId;
        if (!gradeChanged && !jobChanged) return;

        var now = DateTime.UtcNow;
        var open = await context.PositionHistories
            .Where(p => p.UserId == user.Id && p.EndDate == null)
            .OrderByDescending(p => p.StartDate)
            .FirstOrDefaultAsync(cancellationToken);
        if (open != null) open.EndDate = now;

        var jobTitle = user.JobId == null ? null
            : await context.Jobs.Where(j => j.Id == user.JobId).Select(j => j.Title).FirstOrDefaultAsync(cancellationToken);

        context.PositionHistories.Add(new PositionHistory
        {
            UserId       = user.Id,
            JobTitle     = jobTitle,
            CompanyName  = user.CompanyName,
            DepartmentId = user.DepartmentId,
            TeamId       = user.TeamId,
            Grade        = user.Grade,
            StartDate    = open?.EndDate ?? now,
            EndDate      = null,
            ChangeType   = (gradeChanged && jobChanged) ? "Title & Grade Upgrade"
                         : gradeChanged ? "Grade Upgrade" : "Title Change"
        });
    }

    public async Task UpdateMyContactInfoAsync(int userId, UpdateContactInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);
        user.PhoneNumber         = dto.WorkPhoneNumber;
        user.PersonalEmail       = dto.PersonalEmail;
        user.PersonalPhoneNumber = dto.PersonalPhoneNumber;
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
        var baseQuery = context.Users.AsNoTracking().Where(u => u.IsActive);
        var total = await baseQuery.CountAsync(cancellationToken);
        var items = await baseQuery
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(u => new UserSummaryDto
            {
                Id              = u.Id,
                Slug            = u.Slug,
                FirstName       = u.FirstName,
                LastName        = u.LastName,
                AvatarColorIndex = u.AvatarColorIndex,
                Email           = u.Email,
                PhoneNumber     = u.PhoneNumber,
                City            = u.City,
                Country         = u.Country,
                IsActive        = u.IsActive,
                UserStartDate   = u.UserStartDate,
                DepartmentId           = u.DepartmentId,
                OrganizationPositionId = u.OrganizationPositionId,
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

    public async Task<EmployeeCardDto?> GetEmployeeCardAsync(int id, CancellationToken cancellationToken = default)
        => await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<EmployeeCardDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<EmployeeCardDto?> GetEmployeeCardBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => await context.Users
            .AsNoTracking()
            .Where(u => u.Slug == slug)
            .ProjectTo<EmployeeCardDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<string> GenerateUniqueSlugAsync(string? firstName, string? lastName, CancellationToken cancellationToken = default)
    {
        var baseSlug = SlugGenerator.FromName(firstName, lastName);
        var slug = baseSlug;
        var suffix = 2;
        while (await context.Users.AnyAsync(u => u.Slug == slug, cancellationToken))
            slug = $"{baseSlug}-{suffix++}";
        return slug;
    }

    public async Task<UserAvatarDto?> GetAvatarByUserIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.UserAvatars
            .AsNoTracking()
            .Where(a => a.UserId == id)
            .ProjectTo<UserAvatarDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<EmployeeSensitiveInfoDto?> GetSensitiveInfoAsync(int id, CancellationToken cancellationToken = default)
    {
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
                LegalAddressCity    = u.LegalAddressCity,
                LegalAddressCountry = u.LegalAddressCountry,
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

        var options    = onboardingOptions.Value;
        var isFirstUser = !await context.Users.AnyAsync(cancellationToken);
        var isBypassed  = normalizedEmail is not null
                          && options.BypassEmails.Any(e => string.Equals(e?.Trim(), email?.Trim(), StringComparison.OrdinalIgnoreCase));
        var startAsActive = isFirstUser || isBypassed || hasElevatedRole || !options.AutoStartEnabled;

        var newUser = new User
        {
            EntraObjectId   = objectId,
            Email           = email,
            NormalizedEmail = normalizedEmail,
            FirstName       = firstName,
            LastName        = lastName,
            Slug            = await GenerateUniqueSlugAsync(firstName, lastName, cancellationToken),
            IsActive        = startAsActive,
            LifecycleStatus = startAsActive ? UserLifecycleStatuses.Active : UserLifecycleStatuses.Onboarding
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

    private static async Task<int?> ResolveTypeIdAsync<T>(Microsoft.EntityFrameworkCore.DbSet<T> set, string? name, CancellationToken ct)
        where T : NamedTypeEntity
        => string.IsNullOrWhiteSpace(name)
            ? null
            : await set.Where(t => t.Name == name).Select(t => (int?)t.Id).FirstOrDefaultAsync(ct);

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        => ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx
           && (sqlEx.Number == 2627 || sqlEx.Number == 2601);

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
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

    public async Task UpdateAvatarColorAsync(int userId, int? colorIndex, CancellationToken cancellationToken = default)
    {
        if (colorIndex is < 0)
            throw new DomainValidationException("Avatar colour index must be non-negative.");

        var user = await RequireUserAsync(userId, cancellationToken);
        user.AvatarColorIndex = colorIndex;
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

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        user.OrganizationPositionId = organizationPositionId;
        user.ReportsToId            = null;
        await context.SaveChangesAsync(cancellationToken);
        await reportsToCalculator.RecalculateAsync(cancellationToken);        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateCareerAssignmentAsync(int userId, UpdateCareerAssignmentDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        if (dto.DepartmentId.HasValue && !await context.Departments.AnyAsync(d => d.Id == dto.DepartmentId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(Department), dto.DepartmentId.Value);

        if (dto.TeamId.HasValue && !await context.Teams.AnyAsync(t => t.Id == dto.TeamId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(Team), dto.TeamId.Value);

        if (dto.JobId.HasValue && !await context.Jobs.AnyAsync(j => j.Id == dto.JobId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(Job), dto.JobId.Value);

        if (dto.CareerPathId.HasValue && !await context.CareerPaths.AnyAsync(p => p.Id == dto.CareerPathId.Value, cancellationToken))
            throw new EntityNotFoundException(nameof(CareerPath), dto.CareerPathId.Value);

        var prevGrade = user.Grade;
        var prevJobId = user.JobId;

        user.CompanyName  = dto.CompanyName;
        user.DepartmentId = dto.DepartmentId;
        user.TeamId       = dto.TeamId;
        user.CareerPathId = dto.CareerPathId;
        user.JobId        = dto.JobId;
        user.Grade        = dto.Grade;

        await SnapshotPositionIfChangedAsync(user, prevGrade, prevJobId, cancellationToken);

        if (!dto.ManagerId.HasValue)
        {
            await context.SaveChangesAsync(cancellationToken);
            return;
        }

        await WireEmployeeUnderManagerAsync(user, dto.ManagerId.Value, dto.NewPositionName, cancellationToken);
    }

    private async Task WireEmployeeUnderManagerAsync(User employee, int managerId, string? newPositionName, CancellationToken cancellationToken)
    {
        if (managerId == employee.Id)
            throw new DomainValidationException("A user cannot be their own manager.");

        var manager = await context.Users.AsNoTracking()
            .Where(u => u.Id == managerId)
            .Select(u => new { u.OrganizationPositionId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new EntityNotFoundException("User (manager)", managerId);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        if (manager.OrganizationPositionId is not int managerPositionId)
        {
            var orphanedPositionId = employee.OrganizationPositionId;
            employee.OrganizationPositionId = null;
            employee.ReportsToId            = managerId;            await context.SaveChangesAsync(cancellationToken);

            if (orphanedPositionId is int posId)
            {
                await RemoveEmptyLeafPositionAsync(posId, cancellationToken);
                await reportsToCalculator.RecalculateAsync(cancellationToken);
            }
            await transaction.CommitAsync(cancellationToken);
            return;
        }

        if (employee.OrganizationPositionId is int employeePositionId)
        {
            if (managerPositionId == employeePositionId || await IsDescendantAsync(employeePositionId, managerPositionId, cancellationToken))
                throw new DomainConflictException("Cannot place the manager's position beneath the employee's own position — circular dependency.");

            var employeePosition = await context.OrganizationPositions.FirstAsync(p => p.Id == employeePositionId, cancellationToken);
            employeePosition.ParentId = managerPositionId;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(newPositionName))
                throw new DomainValidationException("A position name is required to add the employee to the org chart.");

            var newPosition = new OrganizationPosition { Name = newPositionName.Trim(), ParentId = managerPositionId };
            context.OrganizationPositions.Add(newPosition);
            await context.SaveChangesAsync(cancellationToken);            employee.OrganizationPositionId = newPosition.Id;
        }

        employee.ReportsToId = null;        await context.SaveChangesAsync(cancellationToken);
        await reportsToCalculator.RecalculateAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    private async Task RemoveEmptyLeafPositionAsync(int positionId, CancellationToken cancellationToken)
    {
        var hasUsers    = await context.Users.AnyAsync(u => u.OrganizationPositionId == positionId, cancellationToken);
        var hasChildren = await context.OrganizationPositions.AnyAsync(p => p.ParentId == positionId, cancellationToken);
        if (hasUsers || hasChildren) return;

        var position = await context.OrganizationPositions.FirstOrDefaultAsync(p => p.Id == positionId, cancellationToken);
        if (position is null) return;

        context.OrganizationPositions.Remove(position);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<bool> IsDescendantAsync(int ancestorId, int potentialDescendantId, CancellationToken cancellationToken)
    {
        var parentMap = await context.OrganizationPositions.AsNoTracking()
            .Select(p => new { p.Id, p.ParentId })
            .ToDictionaryAsync(p => p.Id, p => p.ParentId, cancellationToken);

        var currentId = (int?)potentialDescendantId;
        var guard = 0;
        while (currentId.HasValue && parentMap.TryGetValue(currentId.Value, out var parentId))
        {
            if (parentId == ancestorId) return true;
            currentId = parentId;
            if (++guard > parentMap.Count) break;
        }
        return false;
    }

    public async Task<List<UserSearchResultDto>> SearchUsersAsync(string? query, int take, CancellationToken cancellationToken = default)
    {
        take = Math.Clamp(take, 1, 50);

        var users = context.Users.AsNoTracking().Where(u => u.IsActive);
        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = query.Trim();
            users = users.Where(u =>
                (u.FirstName != null && u.FirstName.Contains(term)) ||
                (u.LastName  != null && u.LastName.Contains(term))  ||
                (u.Email     != null && u.Email.Contains(term)));
        }

        return await users
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
            .Take(take)
            .Select(u => new UserSearchResultDto
            {
                Id                     = u.Id,
                Slug                   = u.Slug,
                FirstName              = u.FirstName,
                LastName               = u.LastName,
                Email                  = u.Email,
                AvatarColorIndex       = u.AvatarColorIndex,
                AvatarContentType      = u.Avatar != null ? u.Avatar.ContentType : null,
                AvatarBase64           = u.Avatar != null ? u.Avatar.ContentBase64 : null,
                CompanyName            = u.CompanyName,
                DepartmentId           = u.DepartmentId,
                TeamId                 = u.TeamId,
                OrganizationPositionId = u.OrganizationPositionId,
            })
            .ToListAsync(cancellationToken);
    }


    private async Task<User> RequireUserAsync(int userId, CancellationToken cancellationToken = default)
        => await context.Users.FindAsync([userId], cancellationToken)
           ?? throw new EntityNotFoundException("User", userId);

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

    private async Task<User> RequireUserWithAsync(int userId,
        System.Linq.Expressions.Expression<Func<User, object?>> include,
        CancellationToken cancellationToken = default)
        => await context.Users
               .Include(include)
               .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
           ?? throw new EntityNotFoundException("User", userId);

}
