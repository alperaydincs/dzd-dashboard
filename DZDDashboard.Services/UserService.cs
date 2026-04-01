
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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

            public async Task<bool> UpdateContactsAsync(int userId, UpdateContactsDto dto)
            {
                EnsureValidEmail(dto.Email, "Work email");
                EnsureValidEmail(dto.PersonalEmail, "Personal email");

                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;
                user.Email = dto.Email;
                user.PhoneNumber = dto.PhoneNumber;
                user.PersonalEmail = dto.PersonalEmail;
                user.PersonalPhoneNumber = dto.PersonalPhoneNumber;
                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<bool> UpdateCitizenshipInfoAsync(int userId, UpdateCitizenshipInfoDto dto)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;
                user.DateOfBirth = dto.DateOfBirth;
                user.Gender = dto.Gender;
                user.Nationality = dto.Nationality;
                user.CitizenshipNumber = dto.CitizenshipNumber;
                user.DisabilityStatus = dto.DisabilityStatus;
                user.DisabilityDegree = dto.DisabilityDegree;
                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<bool> UpdateAddressInfoAsync(int userId, UpdateAddressInfoDto dto)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;
                user.LegalAddress = dto.LegalAddress;
                user.CurrentAddress = dto.CurrentAddress;
                user.City = dto.City;
                user.Country = dto.Country;
                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<bool> UpdateEducationInfoAsync(int userId, UpdateEducationInfoDto dto)
            {
                var user = await _context.Users
                    .Include(u => u.EducationHistories)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) return false;

                var existingEducation = user.EducationHistories ?? new List<EducationHistory>();
                if (existingEducation.Count > 0)
                {
                    _context.EducationHistories.RemoveRange(existingEducation);
                }

                user.EducationHistories = (dto.EducationHistories ?? new List<EducationHistoryDto>())
                    .Where(x => !string.IsNullOrWhiteSpace(x.Level) && !string.IsNullOrWhiteSpace(x.Institution))
                    .Select(x => new EducationHistory
                    {
                        UserId = userId,
                        Level = x.Level,
                        Institution = x.Institution,
                        Program = x.Program,
                        GraduationDate = x.GraduationDate,
                        Status = x.Status
                    })
                    .ToList();

                await _context.SaveChangesAsync();
                return true;
            }

        public async Task<UserProfileDto?> GetProfileByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

            public async Task<List<UserDto>> GetAllWithRolesAsync()
            {
                var users = await _context.Users
                    .Include(u => u.Avatar)
                    .ToListAsync();
                return _mapper.Map<List<UserDto>>(users);
            }

            public async Task<EmployeeCardDto?> GetEmployeeCardAsync(int id)
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .Include(u => u.Avatar)
                    .Include(u => u.Department)
                    .Include(u => u.Team)
                    .Include(u => u.Job)
                    .Include(u => u.PayrollLocation)
                    .Include(u => u.OrganizationPosition)
                    .Include(u => u.ReportsTo)
                        .ThenInclude(r => r!.Department)
                    .Include(u => u.ReportsTo)
                        .ThenInclude(r => r!.Job)
                    .Include(u => u.ReportsTo)
                        .ThenInclude(r => r!.Avatar)
                    .Include(u => u.Children)
                    .Include(u => u.EmergencyContacts)
                    .Include(u => u.EducationHistories)
                    .Include(u => u.TargetEfforts)
                    .Include(u => u.UserTrainings)
                    .FirstOrDefaultAsync(u => u.Id == id);

                return user == null ? null : _mapper.Map<EmployeeCardDto>(user);
            }

        public async Task<bool> UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.RegistrationNumber = dto.RegistrationNumber;
            user.UserStartDate = dto.UserStartDate;
            user.PositionStartDate = dto.PositionStartDate;
            user.ContractType = dto.ContractType;
            user.ContractEndDate = dto.ContractEndDate;
            user.WorkModel = dto.WorkModel;
            user.PayrollLocationId = dto.PayrollLocationId;
            user.ModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
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

            user.AvatarId = user.Avatar.Id;

            await _context.SaveChangesAsync();

            if (user.AvatarId != user.Avatar.Id)
            {
                user.AvatarId = user.Avatar.Id;
                await _context.SaveChangesAsync();
            }

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
            EnsureValidEmail(dto.PersonalEmail, "Personal email");

            var user = await _context.Users.FindAsync(userId);
            if (user is null) return false;

            _mapper.Map(dto, user);
            await _context.SaveChangesAsync();
            return true;
        }

        private static void EnsureValidEmail(string? email, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            var validator = new EmailAddressAttribute();
            if (!validator.IsValid(email))
            {
                throw new InvalidOperationException($"{fieldName} is invalid.");
            }
        }

        public async Task<bool> UpdateEmergencyContactsAsync(UpdateEmergencyContactsDto dto)
        {
            var user = await _context.Users.Include(u => u.EmergencyContacts).FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (user is null) return false;
            user.EmergencyContacts ??= new List<EmergencyContact>();
            var validContacts = dto.EmergencyContacts
                .Where(c => !string.IsNullOrWhiteSpace(c.FullName) && !string.IsNullOrWhiteSpace(c.PhoneNumber) && !string.IsNullOrWhiteSpace(c.Relationship))
                .ToList();
            var contactIdsToKeep = validContacts.Select(c => c.Id).Where(id => id > 0).ToList();
            var contactsToRemove = user.EmergencyContacts.Where(c => !contactIdsToKeep.Contains(c.Id) && c.Id > 0).ToList();
            foreach (var contact in contactsToRemove)
            {
                if (contact.Id > 0)
                    _context.EmergencyContacts.Remove(contact);
            }
            foreach (var contactDto in validContacts)
            {
                if (contactDto.Id > 0)
                {
                    var existingContact = user.EmergencyContacts.FirstOrDefault(c => c.Id == contactDto.Id);
                    if (existingContact != null)
                    {
                        _mapper.Map(contactDto, existingContact);
                    }
                }
                else
                {
                    var newContact = _mapper.Map<EmergencyContact>(contactDto);
                    newContact.UserId = user.Id;
                    user.EmergencyContacts.Add(newContact);
                }
            }
            user.ModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateFamilyInfoAsync(UpdateFamilyInfoDto dto)
        {
            var user = await _context.Users.Include(u => u.Children).FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (user is null) return false;
            user.MaritalStatus = dto.MaritalStatus;
            user.SpouseFullName = dto.SpouseFullName;
            user.Children ??= new List<ChildInfo>();
            var validChildren = dto.Children
                .Where(c => !string.IsNullOrWhiteSpace(c.FullName) && c.DateOfBirth != null)
                .ToList();
            var childIdsToKeep = validChildren.Select(c => c.Id).Where(id => id > 0).ToList();
            var childrenToRemove = user.Children.Where(c => !childIdsToKeep.Contains(c.Id) && c.Id > 0).ToList();
            foreach (var child in childrenToRemove)
            {
                if (child.Id > 0)
                    _context.ChildInfos.Remove(child);
            }
            foreach (var childDto in validChildren)
            {
                if (childDto.Id > 0)
                {
                    var existingChild = user.Children.FirstOrDefault(c => c.Id == childDto.Id);
                    if (existingChild != null)
                    {
                        _mapper.Map(childDto, existingChild);
                    }
                }
                else
                {
                    var newChild = _mapper.Map<ChildInfo>(childDto);
                    newChild.UserId = user.Id;
                    user.Children.Add(newChild);
                }
            }
            user.ModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
