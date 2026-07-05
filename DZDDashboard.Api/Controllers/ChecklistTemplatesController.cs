using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/checklist-templates")]
[Authorize(Roles = Roles.AdminOrHr)]
public class ChecklistTemplatesController(IChecklistTemplateService templates) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<ChecklistStepTemplateDto>>> List([FromQuery] string processType, CancellationToken cancellationToken)
        => Ok(await templates.GetAsync(processType, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<ChecklistStepTemplateDto>> Create([FromBody] ChecklistStepTemplateDto dto, CancellationToken cancellationToken)
        => Ok(await templates.CreateAsync(dto, cancellationToken));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ChecklistStepTemplateDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await templates.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await templates.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
