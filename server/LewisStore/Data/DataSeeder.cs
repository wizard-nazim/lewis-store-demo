using BCrypt.Net;
using LewisStore.Models;

namespace LewisStore.Data;

public static class DataSeeder
{
    public static void Seed(LewisDbContext db)
    {
        if (!db.Users.Any())
        {
            var admin = new User
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Email = "admin@lewis.co.za",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                Balance = 1000m
            };
            var cust = new User
            {
                Id = Guid.NewGuid(),
                Name = "Customer",
                Email = "customer@lewis.co.za",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("cust1234"),
                Role = "Customer",
                Balance = 200m
            };
            db.Users.AddRange(admin, cust);
        }

        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Id = Guid.NewGuid(), Name = "Phone", SKU = "SP-001", UnitPrice = 249.99m, CostPrice = 150, StockQty = 10, IsActive = true },
                new Product { Id = Guid.NewGuid(), Name = "Headphones", SKU = "SH-001", UnitPrice = 49.99m, CostPrice = 20, StockQty = 30, IsActive = true }
            );
        }

        db.SaveChanges();
    }
}
