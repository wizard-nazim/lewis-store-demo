using LewisStore.Dtos;
using LewisStore.Models;
using LewisStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LewisStore.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _svc;
    public OrdersController(IOrderService svc) => _svc = svc;

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CreateOrderDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);

        var order = new Order
        {
            CustomerId = userId,
            DeliveryFee = dto.DeliveryFee,
            Tax = dto.Tax,
            PaymentType = dto.PaymentType,
            Items = dto.Items.Select(i => new OrderItem { ProductId = i.ProductId, Quantity = i.Quantity, UnitPrice = i.UnitPrice, LineTotal = i.UnitPrice * i.Quantity }).ToList()
        };

        try
        {
            var created = await _svc.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _svc.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }
}
