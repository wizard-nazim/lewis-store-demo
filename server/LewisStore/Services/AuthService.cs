using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LewisStore.Data;
using LewisStore.Dtos;
using LewisStore.Models;

namespace LewisStore.Services;

public class AuthService : IAuthService
{
    private readonly LewisDbContext _db;
    private readonly IConfiguration _config;
    public AuthService(LewisDbContext db, IConfiguration config) { _db = db; _config = config; }

    public async Task<(bool Success, string? Token, string? Error)> RegisterAsync(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email)) return (false, null, "Email already in use");
        var user = new User { Name = dto.Name, Email = dto.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), Role = "Customer", Balance = 0m };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        var token = GenerateToken(user);
        return (true, token, null);
    }

    public async Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return (false, null, "Invalid credentials");
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return (false, null, "Invalid credentials");
        var token = GenerateToken(user);
        return (true, token, null);
    }

    public async Task<User?> GetByIdAsync(Guid id) => await _db.Users.FindAsync(id);

    private string GenerateToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("Missing Jwt:Key"));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"] ?? "10080")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
