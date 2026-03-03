using AutoMapper;
using AutoMapper.QueryableExtensions;
using DZDDashboard.Common.DTOs.Users;
using DZDDashboard.Common.Services;
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
                .OrderBy(u => u.Username)
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
        
        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Email and password required.");

            if (await _context.Users.AnyAsync(x => x.NormalizedEmail == dto.Email.ToUpperInvariant()))
                throw new InvalidOperationException("Email already exists.");

            var user = _mapper.Map<User>(dto);
            user.Username = ExtractUsernameFromEmail(dto.Email);
            user.NormalizedUsername = user.Username.ToUpperInvariant();
            user.IsActive = true;
            user.PasswordHash = PasswordHasher.Hash(dto.Password);
            user.NormalizedEmail = dto.Email.ToUpperInvariant();

            _context.Users.Add(user);
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            _context.UserRoles.Add(new UserRole { User = user, Role = defaultRole });
            await _context.SaveChangesAsync();
            var resultDto = _mapper.Map<UserDto>(user);

            return resultDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private static string ExtractUsernameFromEmail(string email)
        {
            var trimmedEmail = email.Trim();
            var atIndex = trimmedEmail.IndexOf('@');

            if (atIndex > 0)
            {
                return trimmedEmail[..atIndex];
            }
            return trimmedEmail;
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
            var user = await _context.Users.Include(u => u.Children).FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) return false;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.CompanyName = dto.CompanyName;
            user.RegistrationNumber = dto.RegistrationNumber;
            user.Grade = dto.Grade;
            user.JobId = dto.JobId;
            user.DepartmentId = dto.DepartmentId;
            user.TeamId = dto.TeamId;
            user.PositionStartDate = dto.PositionStartDate;
            user.UserStartDate = dto.UserStartDate;
            user.ContractType = dto.ContractType;
            user.ContractEndDate = dto.ContractEndDate;
            user.WorkModel = dto.WorkModel;
            user.PayrollLocationId = dto.PayrollLocationId;
            user.ReportsToId = dto.ReportsToId;
            user.ApprovalProcessUnit = dto.ApprovalProcessUnit;

            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.PersonalEmail = dto.PersonalEmail;
            user.PersonalPhoneNumber = dto.PersonalPhoneNumber;

            user.DateOfBirth = dto.DateOfBirth;
            user.Gender = dto.Gender;
            user.DisabilityStatus = dto.DisabilityStatus;
            user.DisabilityDegree = dto.DisabilityDegree;
            user.Nationality = dto.Nationality;
            user.CitizenshipNumber = dto.CitizenshipNumber;

            user.EmergencyContactFullName = dto.EmergencyContactFullName;
            user.EmergencyContactRelationship = dto.EmergencyContactRelationship;
            user.EmergencyContactPhoneNumber = dto.EmergencyContactPhoneNumber;

            user.MaritalStatus = dto.MaritalStatus;
            user.SpouseFullName = dto.SpouseFullName;

            user.LegalAddress = dto.LegalAddress;
            user.CurrentAddress = dto.CurrentAddress;
            user.City = dto.City;
            user.Country = dto.Country;

            user.EducationStatus = dto.EducationStatus;
            user.HighestEducationLevel = dto.HighestEducationLevel;
            user.HighSchoolName = dto.HighSchoolName;
            user.BachelorsUniversityName = dto.BachelorsUniversityName;
            user.BachelorsProgramName = dto.BachelorsProgramName;
            user.MastersUniversityName = dto.MastersUniversityName;
            user.MastersProgramName = dto.MastersProgramName;
            user.BachelorsGraduatedDate = dto.BachelorsGraduatedDate;
            user.MastersGraduatedDate = dto.MastersGraduatedDate;

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

        public async Task UpdateOrganizationPositionAsync(int userId, int? organizationPositionId, int? reportsToId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (organizationPositionId.HasValue)
            {
                var position = await _context.OrganizationPositions.FindAsync(organizationPositionId.Value);
                if (position == null) throw new KeyNotFoundException("Organization Position not found");
            }

            if (reportsToId.HasValue)
            {
                var manager = await _context.Users.FindAsync(reportsToId.Value);
                if (manager == null) throw new KeyNotFoundException("Manager (ReportsTo) not found");
            }

            var oldOrganizationPositionId = user.OrganizationPositionId;
            var oldReportsToId = user.ReportsToId;
            
            user.OrganizationPositionId = organizationPositionId;
            user.ReportsToId = reportsToId;
            
            await _context.SaveChangesAsync();

            if (oldOrganizationPositionId.HasValue && oldOrganizationPositionId != organizationPositionId)
            {
                await ReassignSkipLevelReportsAsync(oldOrganizationPositionId.Value, reportsToId);
            }
        }

        private async Task ReassignSkipLevelReportsAsync(int oldPositionId, int? newManagerId)
        {
            
            var childPositions = await _context.OrganizationPositions
                .Where(p => p.ParentId == oldPositionId)
                .ToListAsync();

            foreach (var childPos in childPositions)
            {
                
                var usersInChildPos = await _context.Users
                    .Where(u => u.OrganizationPositionId == childPos.Id)
                    .ToListAsync();

                if (usersInChildPos.Any() && newManagerId.HasValue)
                {
                    foreach (var childUser in usersInChildPos)
                    {
                        
                        childUser.ReportsToId = newManagerId;
                    }
                }
                else if (!usersInChildPos.Any())
                {
                    
                    await ReassignSkipLevelReportsAsync(childPos.Id, newManagerId);
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateContactInfoAsync(int userId, UpdateContactInfoDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.PhoneNumber = dto.WorkPhoneNumber;
            user.PersonalEmail = dto.PersonalEmail;
            user.PersonalPhoneNumber = dto.PersonalPhoneNumber;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
