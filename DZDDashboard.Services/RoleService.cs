using DZDDashboard.Common.DTOs;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Api.Services
{
    public class RoleService(AppDbContext db)
    {
        private readonly AppDbContext _db = db;

        public async Task<List<RoleDto>> GetAsync()
        {
            return await _db.Roles
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name! })
                .ToListAsync();
        }

        public async Task<RoleDto?> GetByIdAsync(int id)
        {
            var role = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            return role is null ? null : new RoleDto{ Id = role.Id, Name = role.Name! };
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var normalized = name.Trim();
            return await _db.Roles.AnyAsync(r => r.Name != null && r.Name.ToLower() == normalized.ToLower());
        }

        public async Task<RoleDto> CreateAsync(string name)
        {
            var trimmed = name.Trim();
            var entity = new Role { Name = trimmed };
            _db.Roles.Add(entity);
            await _db.SaveChangesAsync();
            return new RoleDto{ Id = entity.Id, Name = entity.Name! };
        }

        public async Task<(RoleDto? Role, string? Error)> UpdateAsync(int id, string name)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role is null) return (null, null);

            var trimmed = name.Trim();
            var duplicate = await _db.Roles.AnyAsync(r => r.Id != id && r.Name != null && r.Name.ToLower() == trimmed.ToLower());
            if (duplicate) return (null, "Another role with that name exists.");

            role.Name = trimmed;
            await _db.SaveChangesAsync();
            return (new RoleDto { Id = role.Id, Name = role.Name! }, null);
        }

        public async Task<(bool Deleted, string? Error, bool NotFound)> DeleteAsync(int id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role is null) return (false, null, true);

            var inUse = await _db.UserRoles.AnyAsync(ur => ur.RoleId == id);
            if (inUse) return (false, "Role is assigned to one or more users.", false);

            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();
            return (true, null, false);
        }
    }
}