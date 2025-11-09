using LewisStore.Dtos;
using LewisStore.Data;
using LewisStore.Models;
using LewisStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LewisStore.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly LewisDbContext _db;
    private readonly IPaymentService _payment;
    public PaymentsController(LewisDbContext db, IPaymentService payment) { _db = db; _payment = payment; }

    [HttpPost("record")]
    public async Task<IActionResult> RecordPayment([FromBody] PaymentDto dto)
    {
        var pay = new Payment { Amount = dto.Amount, OrderId = dto.OrderId, CreditAgreementId = dto.CreditAgreementId, Method = dto.Method ?? "Manual", Reference = dto.Reference };
        _db.Payments.Add(pay);
        await _db.SaveChangesAsync();
        return Ok(pay);
    }

    // create a mock payment session (simulate redirect to provider)
    [HttpPost("create-session")]
    public async Task<IActionResult> CreateSession([FromBody] TopUpDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);

        var sessionId = await _payment.CreatePaymentSessionAsync(userId, dto.Amount, "mock://callback");
        return Ok(new { sessionId });
    }

    // This endpoint lets you simulate provider webhook success:
    // call POST /api/payments/webhook/simulate with { "sessionId": "guid-string" }
    [AllowAnonymous]
    [HttpPost("webhook/simulate")]
    public async Task<IActionResult> SimulateWebhook([FromBody] SimulateWebhookDto dto)
    {
        if (!Guid.TryParse(dto.SessionId, out var sid)) return BadRequest("Invalid session id");
        var ok = await (_payment as MockPaymentService)!.SimulateWebhookPaymentSucceededAsync(sid);
        if (!ok) return BadRequest("Session not found or failed");
        return Ok(new { success = true });
    }
}

public record SimulateWebhookDto(string SessionId);
