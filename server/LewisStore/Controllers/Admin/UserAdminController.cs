using LewisStore.Data;
using LewisStore.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LewisStore.Controllers.Admin;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class UserAdminController : ControllerBase
{
    private readonly LewisDbContext _db;
    public UserAdminController(LewisDbContext db) => _db = db;

    [HttpPost("{id}/balance")]
    public async Task<IActionResult> AddBalance(Guid id, [FromBody] TopUpDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        if (dto.Amount <= 0) return BadRequest("Amount must be > 0");
        user.Balance += dto.Amount;
        await _db.SaveChangesAsync();
        return Ok(new { user.Id, user.Email, NewBalance = user.Balance });
    }

    [HttpGet]
    public IActionResult ListAllUsers() => Ok(_db.Users.Select(u => new { u.Id, u.Email, u.Name, u.Role, u.Balance }));
}
