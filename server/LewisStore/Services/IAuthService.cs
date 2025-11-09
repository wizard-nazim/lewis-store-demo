using LewisStore.Dtos;
using LewisStore.Models;

namespace LewisStore.Services;

public interface IAuthService
{
    Task<(bool Success, string? Token, string? Error)> RegisterAsync(RegisterDto dto);
    Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginDto dto);
    Task<User?> GetByIdAsync(Guid id);
}
