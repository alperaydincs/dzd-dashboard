using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DZDDashboard.Api.Services;

namespace DZDDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RolesController(RoleService roleService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var roles = await roleService.GetAsync();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Role name cannot be empty.");

            if (await roleService.ExistsByNameAsync(dto.Name))
                return Conflict("Role already exists.");

            var created = await roleService.CreateAsync(dto.Name);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Role name cannot be empty.");

            var result = await roleService.UpdateAsync(id, dto.Name);
            if (result.Role is null && result.Error is null)
                return NotFound();

            if (result.Error is not null)
                return Conflict(result.Error);

            return Ok(result.Role);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await roleService.DeleteAsync(id);
            if (result.NotFound) return NotFound();
            if (!result.Deleted)
                return Conflict(result.Error ?? "Unable to delete role.");

            return NoContent();
        }
    }
}
