using DZDDashboard.Api.Abstractions;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/trainings")]
public class TrainingsController(
    ITrainingProgressService trainingProgress,
    ICurrentUserAccessor currentUser) : BaseController
{
    /// <summary>Udemy training progress for the signed-in employee.</summary>
    [HttpGet("my-progress")]
    public async Task<ActionResult<TrainingProgressSummaryDto>> GetMyProgress(CancellationToken cancellationToken)
    {
        var id = currentUser.UserId;
        if (id is null) return Unauthorized();
        return Ok(await trainingProgress.GetForUserAsync(id.Value, cancellationToken));
    }

    /// <summary>Udemy training progress for a specific employee (Admin/HR only).</summary>
    [HttpGet("users/{userId:int}/progress")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<TrainingProgressSummaryDto>> GetUserProgress(int userId, CancellationToken cancellationToken)
        => Ok(await trainingProgress.GetForUserAsync(userId, cancellationToken));
}
