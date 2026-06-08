using DZDDashboard.Api.Abstractions;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

/// <summary>
/// Employee Profile → Payment screen: salary history, benefit line items (incl. ÖSS
/// dependents) and additional payments.
/// Edit operations are Admin/HR only (per the analysis doc's role matrix — "İK Operasyon"
/// enters/updates pay and benefits, "Finans/Bordro" tracks additional payments).
/// Employees get a reduced, view-only summary of their own salary + BES via
/// <c>GET api/users/my-profile/payment-summary</c> (decision: "Maaş ve BES görebilir ama
/// düzenleyemez, sadece kendine ait olanı görür").
/// </summary>
[Route("api/users")]
public class PaymentController(IPaymentService paymentService, ICurrentUserAccessor currentUser) : BaseController
{
    [HttpGet("my-profile/payment-summary")]
    public async Task<ActionResult<MyPaymentSummaryDto>> GetMyPaymentSummary(CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (!userId.HasValue) return Unauthorized();

        return Ok(await paymentService.GetMyPaymentSummaryAsync(userId.Value, cancellationToken));
    }

    [HttpGet("{id:int}/payment")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<EmployeePaymentDto>> GetEmployeePayment(int id, CancellationToken cancellationToken)
        => Ok(await paymentService.GetEmployeePaymentAsync(id, cancellationToken));

    // ── Salary ───────────────────────────────────────────────────────────────

    [HttpPost("{id:int}/payment/salary")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<SalaryRecordDto>> CreateSalaryRecord(int id, [FromBody] SalaryRecordDto dto, CancellationToken cancellationToken)
    {
        var result = await paymentService.CreateSalaryRecordAsync(id, dto, cancellationToken);
        return CreatedAtAction(nameof(GetEmployeePayment), new { id }, result);
    }

    [HttpPut("{id:int}/payment/salary/{recordId:int}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> UpdateSalaryRecord(int id, int recordId, [FromBody] SalaryRecordDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(recordId, dto.Id) is { } mismatch) return mismatch;
        await paymentService.UpdateSalaryRecordAsync(id, recordId, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}/payment/salary/{recordId:int}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> DeleteSalaryRecord(int id, int recordId, CancellationToken cancellationToken)
    {
        await paymentService.DeleteSalaryRecordAsync(id, recordId, cancellationToken);
        return NoContent();
    }

    // ── Benefits ─────────────────────────────────────────────────────────────

    [HttpPost("{id:int}/payment/benefits")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<BenefitRecordDto>> CreateBenefitRecord(int id, [FromBody] BenefitRecordDto dto, CancellationToken cancellationToken)
    {
        var result = await paymentService.CreateBenefitRecordAsync(id, dto, cancellationToken);
        return CreatedAtAction(nameof(GetEmployeePayment), new { id }, result);
    }

    [HttpPut("{id:int}/payment/benefits/{recordId:int}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> UpdateBenefitRecord(int id, int recordId, [FromBody] BenefitRecordDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(recordId, dto.Id) is { } mismatch) return mismatch;
        await paymentService.UpdateBenefitRecordAsync(id, recordId, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}/payment/benefits/{recordId:int}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> DeleteBenefitRecord(int id, int recordId, CancellationToken cancellationToken)
    {
        await paymentService.DeleteBenefitRecordAsync(id, recordId, cancellationToken);
        return NoContent();
    }

    // ── Additional Payments ──────────────────────────────────────────────────

    [HttpPost("{id:int}/payment/additional-payments")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<AdditionalPaymentDto>> CreateAdditionalPayment(int id, [FromBody] AdditionalPaymentDto dto, CancellationToken cancellationToken)
    {
        var result = await paymentService.CreateAdditionalPaymentAsync(id, dto, cancellationToken);
        return CreatedAtAction(nameof(GetEmployeePayment), new { id }, result);
    }

    [HttpPut("{id:int}/payment/additional-payments/{paymentId:int}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> UpdateAdditionalPayment(int id, int paymentId, [FromBody] AdditionalPaymentDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(paymentId, dto.Id) is { } mismatch) return mismatch;
        await paymentService.UpdateAdditionalPaymentAsync(id, paymentId, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}/payment/additional-payments/{paymentId:int}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> DeleteAdditionalPayment(int id, int paymentId, CancellationToken cancellationToken)
    {
        await paymentService.DeleteAdditionalPaymentAsync(id, paymentId, cancellationToken);
        return NoContent();
    }
}
