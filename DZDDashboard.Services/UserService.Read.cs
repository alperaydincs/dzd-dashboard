using DZDDashboard.Common.DTOs;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public partial class UserService
{
    public async Task<UserProfileDto?> GetProfileByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectToType<UserProfileDto>(mapper.Config)
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
                IsActive        = u.IsActive,
                UserStartDate   = u.UserStartDate,
                OrganizationPositionId = u.OrganizationPositionId,
                Avatar = u.Avatar == null ? null : new UserAvatarSummaryDto
                {
                    Id          = u.Avatar.Id,
                    ContentType = u.Avatar.ContentType,
                    UpdatedAt   = u.Avatar.ModifiedAt ?? u.Avatar.CreatedAt
                },
                Department = u.Department == null ? null : new DepartmentDto
                {
                    Id        = u.Department.Id,
                    Name      = u.Department.Name,
                    CompanyId = u.Department.CompanyId
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

    public async Task<EmployeeDto?> GetEmployeeCardAsync(int id, CancellationToken cancellationToken = default)
        => await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectToType<EmployeeDto>(mapper.Config)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<EmployeeDto?> GetEmployeeCardBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => await context.Users
            .AsNoTracking()
            .Where(u => u.Slug == slug)
            .ProjectToType<EmployeeDto>(mapper.Config)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<UserAvatarDto?> GetAvatarByUserIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var avatarId = await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => u.AvatarId)
            .FirstOrDefaultAsync(cancellationToken);
        if (avatarId is not int fileId) return null;

        var file = await fileStorage.GetAsync(fileId, cancellationToken);
        if (file is null) return null;

        return new UserAvatarDto
        {
            ContentBase64 = Convert.ToBase64String(file.Value.Content),
            ContentType   = file.Value.ContentType
        };
    }

    public async Task<EmployeeSensitiveInfoDto?> GetSensitiveInfoAsync(int id, CancellationToken cancellationToken = default)
        => await context.Users
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
                CurrentAddressChangedAt = u.CurrentAddressChangedAt,
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
                HasAvatar              = u.Avatar != null,
                AvatarUpdatedAt        = u.Avatar != null ? (u.Avatar.ModifiedAt ?? u.Avatar.CreatedAt) : null,
                CompanyId              = u.CompanyId,
                DepartmentId           = u.DepartmentId,
                TeamId                 = u.TeamId,
                OrganizationPositionId = u.OrganizationPositionId,
            })
            .ToListAsync(cancellationToken);
    }
}
