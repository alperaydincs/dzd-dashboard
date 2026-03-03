using DZDDashboard.Common.DTOs.Organization;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrganizationPositionController : ControllerBase
{
    private readonly OrganizationPositionService _service;

    public OrganizationPositionController(OrganizationPositionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrganizationPositionDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPost]
    public async Task<ActionResult<OrganizationPositionDto>> Create(CreateOrganizationPositionDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrganizationPositionDto>> Update(int id, UpdateOrganizationPositionDto dto)
    {
        if (id != dto.Id) return BadRequest("Id mismatch");
        try
        {
            return Ok(await _service.UpdateAsync(dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
