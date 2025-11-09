using LewisStore.Dtos;
using LewisStore.Models;
using LewisStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LewisStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _svc;
    public ProductsController(IProductService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? q) => Ok(await _svc.GetAllAsync(q));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var p = await _svc.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var p = new Product { Name = dto.Name, SKU = dto.SKU, UnitPrice = dto.UnitPrice, StockQty = dto.StockQty, IsActive = true };
        var created = await _svc.CreateAsync(p);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
