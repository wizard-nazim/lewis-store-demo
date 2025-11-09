using LewisStore.Dtos;
using LewisStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace LewisStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var (success, token, error) = await _auth.RegisterAsync(dto);
        if (!success) return BadRequest(new { error });
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (success, token, error) = await _auth.LoginAsync(dto);
        if (!success) return Unauthorized(new { error });
        return Ok(new { token });
    }
}
