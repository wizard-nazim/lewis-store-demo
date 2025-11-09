using System.ComponentModel.DataAnnotations;

namespace LewisStore.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required] public string Name { get; set; } = "";
    [Required] public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required] public string Role { get; set; } = "Customer"; // Admin | Customer
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public decimal Balance { get; set; } = 0m;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
