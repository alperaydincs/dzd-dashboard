using AutoMapper;
using AutoMapper.QueryableExtensions;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services
{
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public UserService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllWithRolesAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.Email)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<UserDto?> GetByIdWithRolesAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeDetailDto?> GetEmployeeDetailsAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .ProjectTo<EmployeeDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<UserProfileDto?> GetProfileByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<UserAvatarDto?> GetAvatarByUserIdAsync(int id)
        {
            return await _context.UserAvatars
                .AsNoTracking()
                .Where(a => a.UserId == id)
                .ProjectTo<UserAvatarDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PersonalInfoDto?> GetPersonalInfoAsync(int id)
        {
            var user = await _context.Users
            .AsNoTracking()
                .Include(u => u.Children)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
            return user == null ? null : _mapper.Map<PersonalInfoDto>(user);
        }

        public async Task<bool> UpdatePersonalInfoAsync(int id, PersonalInfoDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Children)
                .FirstOrDefaultAsync(u => u.Id == id);
            
            if (user is null) return false;

            _mapper.Map(dto, user);
            user.ModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAvatarAsync(int userId, string contentType, string base64Content)
        {
            var user = await _context.Users.Include(u => u.Avatar).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            if (user.Avatar == null)
            {
                user.Avatar = new UserAvatar
                {
                    UserId = userId,
                    ContentType = contentType,
                    ContentBase64 = base64Content,
                    ModifiedAt = DateTime.UtcNow
                };
                _context.UserAvatars.Add(user.Avatar);
            }
            else
            {
                user.Avatar.ContentType = contentType;
                user.Avatar.ContentBase64 = base64Content;
                user.Avatar.ModifiedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrganizationPositionAsync(int userId, int? organizationPositionId, int? _)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (organizationPositionId.HasValue)
            {
                var position = await _context.OrganizationPositions.FindAsync(organizationPositionId.Value);
                if (position == null) throw new KeyNotFoundException("Organization Position not found");
            }

            user.OrganizationPositionId = organizationPositionId;
            user.ReportsToId = null;

            await RecalculateReportsToAsync();
            await _context.SaveChangesAsync();
        }

        private async Task RecalculateReportsToAsync()
        {
            var positions = await _context.OrganizationPositions
                .AsNoTracking()
                .Select(p => new { p.Id, p.ParentId })
                .ToListAsync();

            var parentByPositionId = positions.ToDictionary(x => x.Id, x => x.ParentId);

            var allUsers = await _context.Users.ToListAsync();

            var positionedUsers = allUsers
                .Where(u => u.OrganizationPositionId.HasValue)
                .ToList();

            var usersByPosition = positionedUsers
                .GroupBy(u => u.OrganizationPositionId!.Value)
                .ToDictionary(g => g.Key, g => g.OrderBy(u => u.Id).ToList());

            foreach (var currentUser in positionedUsers)
            {
                var managerId = FindNearestAncestorManagerId(
                    currentUser.OrganizationPositionId!.Value,
                    currentUser.Id,
                    parentByPositionId,
                    usersByPosition);

                currentUser.ReportsToId = managerId;
            }

            var usersOutsideTree = allUsers
                .Where(u => !u.OrganizationPositionId.HasValue && u.ReportsToId != null)
                .ToList();

            foreach (var userOutsideTree in usersOutsideTree)
            {
                userOutsideTree.ReportsToId = null;
            }
        }

        private static int? FindNearestAncestorManagerId(
            int positionId,
            int currentUserId,
            IReadOnlyDictionary<int, int?> parentByPositionId,
            IReadOnlyDictionary<int, List<User>> usersByPosition)
        {
            if (!parentByPositionId.TryGetValue(positionId, out var parentId))
            {
                return null;
            }

            while (parentId.HasValue)
            {
                if (usersByPosition.TryGetValue(parentId.Value, out var managers))
                {
                    var manager = managers.FirstOrDefault(u => u.Id != currentUserId);
                    if (manager != null)
                    {
                        return manager.Id;
                    }
                }

                if (!parentByPositionId.TryGetValue(parentId.Value, out parentId))
                {
                    break;
                }
            }

            return null;
        }

        public async Task<bool> UpdateContactInfoAsync(int userId, UpdateContactInfoDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null) return false;

            _mapper.Map(dto, user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
