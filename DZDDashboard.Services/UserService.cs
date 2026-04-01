using AutoMapper;
using AutoMapper.QueryableExtensions;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DZDDashboard.Services;

public class UserService
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public UserService(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
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
            _context.EducationHistories.RemoveRange(existingEducation);

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

    public async Task<bool> UpdateContactInfoAsync(int userId, UpdateContactInfoDto dto)
    {
        EnsureValidEmail(dto.PersonalEmail, "Personal email");

        var user = await _context.Users.FindAsync(userId);
        if (user is null) return false;

        _mapper.Map(dto, user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateEmergencyContactsAsync(UpdateEmergencyContactsDto dto)
    {
        var user = await _context.Users
            .Include(u => u.EmergencyContacts)
            .FirstOrDefaultAsync(u => u.Id == dto.UserId);
        if (user is null) return false;

        user.EmergencyContacts ??= new List<EmergencyContact>();

        var validContacts = dto.EmergencyContacts
            .Where(c => !string.IsNullOrWhiteSpace(c.FullName)
                     && !string.IsNullOrWhiteSpace(c.PhoneNumber)
                     && !string.IsNullOrWhiteSpace(c.Relationship))
            .ToList();

        var contactIdsToKeep = validContacts.Select(c => c.Id).Where(id => id > 0).ToList();

        foreach (var contact in user.EmergencyContacts.Where(c => !contactIdsToKeep.Contains(c.Id)).ToList())
            _context.EmergencyContacts.Remove(contact);

        foreach (var contactDto in validContacts)
        {
            if (contactDto.Id > 0)
            {
                var existing = user.EmergencyContacts.FirstOrDefault(c => c.Id == contactDto.Id);
                if (existing != null)
                    _mapper.Map(contactDto, existing);
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
        var user = await _context.Users
            .Include(u => u.Children)
            .FirstOrDefaultAsync(u => u.Id == dto.UserId);
        if (user is null) return false;

        user.MaritalStatus = dto.MaritalStatus;
        user.SpouseFullName = dto.SpouseFullName;
        user.Children ??= new List<ChildInfo>();

        var validChildren = dto.Children
            .Where(c => !string.IsNullOrWhiteSpace(c.FullName) && c.DateOfBirth != null)
            .ToList();

        var childIdsToKeep = validChildren.Select(c => c.Id).Where(id => id > 0).ToList();

        foreach (var child in user.Children.Where(c => !childIdsToKeep.Contains(c.Id)).ToList())
            _context.ChildInfos.Remove(child);

        foreach (var childDto in validChildren)
        {
            if (childDto.Id > 0)
            {
                var existing = user.Children.FirstOrDefault(c => c.Id == childDto.Id);
                if (existing != null)
                    _mapper.Map(childDto, existing);
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
        var user = await _context.Users
            .Include(u => u.Avatar)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return false;

        if (user.Avatar == null)
        {
            var avatar = new UserAvatar
            {
                UserId = userId,
                ContentType = contentType,
                ContentBase64 = base64Content,
                ModifiedAt = DateTime.UtcNow
            };
            _context.UserAvatars.Add(avatar);
            await _context.SaveChangesAsync(); // DB avatar.Id'yi atar

            user.Avatar = avatar;
            user.AvatarId = avatar.Id;
            await _context.SaveChangesAsync(); // User'daki AvatarId FK'sını güncelle
        }
        else
        {
            user.Avatar.ContentType = contentType;
            user.Avatar.ContentBase64 = base64Content;
            user.Avatar.ModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task UpdateOrganizationPositionAsync(int userId, int? organizationPositionId)
    {
        var user = await _context.Users.FindAsync(userId)
            ?? throw new KeyNotFoundException("User not found");

        if (organizationPositionId.HasValue)
        {
            var position = await _context.OrganizationPositions.FindAsync(organizationPositionId.Value);
            if (position == null) throw new KeyNotFoundException("Organization Position not found");
        }

        user.OrganizationPositionId = organizationPositionId;
        user.ReportsToId = null;

        await ReportsToCalculator.RecalculateAsync(_context);
        await _context.SaveChangesAsync();
    }

    private static void EnsureValidEmail(string? email, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;

        var validator = new EmailAddressAttribute();
        if (!validator.IsValid(email))
            throw new InvalidOperationException($"{fieldName} is invalid.");
    }
}
