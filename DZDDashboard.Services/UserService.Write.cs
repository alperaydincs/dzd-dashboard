using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public partial class UserService
{
    public async Task UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        await EnsureExistsAsync<PayrollLocation>(dto.PayrollLocationId, cancellationToken);

        var nameChanged = user.FirstName != dto.FirstName || user.LastName != dto.LastName;

        user.FirstName           = dto.FirstName;
        user.LastName            = dto.LastName;
        user.RegistrationNumber  = dto.RegistrationNumber;
        user.UserStartDate       = dto.UserStartDate;
        user.PositionStartDate   = dto.PositionStartDate;
        user.ContractType        = dto.ContractType;
        user.ContractEndDate     = dto.ContractEndDate;
        user.WorkModel           = dto.WorkModel;
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

        user.EducationHistories = (dto.EducationHistories ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.EducationLevel) && !string.IsNullOrWhiteSpace(x.Institution))
            .Select(x => new EducationHistory
            {
                UserId           = userId,
                EducationLevel   = x.EducationLevel,
                Institution      = x.Institution,
                Program          = x.Program,
                GraduationDate   = x.GraduationDate,
                Status           = x.Status
            })
            .ToList();

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePositionHistoryAsync(int userId, UpdatePositionHistoryDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        await EnsureExistsAsync<Department>(dto.DepartmentId, cancellationToken);
        await EnsureExistsAsync<Team>(dto.TeamId, cancellationToken);

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

        await EnsureExistsAsync<OrganizationPosition>(organizationPositionId, cancellationToken);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        user.OrganizationPositionId = organizationPositionId;
        user.ReportsToId            = null;
        await context.SaveChangesAsync(cancellationToken);
        await reportsToCalculator.RecalculateAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateCareerAssignmentAsync(int userId, UpdateCareerAssignmentDto dto, CancellationToken cancellationToken = default)
    {
        var user = await RequireUserAsync(userId, cancellationToken);

        await EnsureExistsAsync<Department>(dto.DepartmentId, cancellationToken);
        await EnsureExistsAsync<Team>(dto.TeamId, cancellationToken);
        await EnsureExistsAsync<Job>(dto.JobId, cancellationToken);
        await EnsureExistsAsync<CareerPath>(dto.CareerPathId, cancellationToken);

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
            ChangeType   = (gradeChanged && jobChanged) ? PositionChangeTypes.TitleAndGradeUpgrade
                         : gradeChanged ? PositionChangeTypes.GradeUpgrade : PositionChangeTypes.TitleChange
        });
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
            employee.ReportsToId            = managerId;
            await context.SaveChangesAsync(cancellationToken);

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
            await context.SaveChangesAsync(cancellationToken);
            employee.OrganizationPositionId = newPosition.Id;
        }

        employee.ReportsToId = null;
        await context.SaveChangesAsync(cancellationToken);
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

    /// <summary>
    /// Loads the position parent graph and delegates the ancestry rule to the
    /// <see cref="OrganizationHierarchy"/> domain object.
    /// </summary>
    private async Task<bool> IsDescendantAsync(int ancestorId, int potentialDescendantId, CancellationToken cancellationToken)
    {
        var parentMap = await context.OrganizationPositions.AsNoTracking()
            .Select(p => new { p.Id, p.ParentId })
            .ToDictionaryAsync(p => p.Id, p => p.ParentId, cancellationToken);

        return new OrganizationHierarchy(parentMap).IsDescendant(ancestorId, potentialDescendantId);
    }
}
