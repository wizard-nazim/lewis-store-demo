using LewisStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LewisStore.Data
{
    public static class DataSeeder
    {
        public static void Seed(LewisDbContext context, IServiceProvider services)
        {
            if (context.Users.Any()) return; // prevent reseeding

            var admin = new User
            {
                Email = "admin@lewis.co.za",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                Balance = 1000
            };

            var customer = new User
            {
                Email = "customer@lewis.co.za",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer1234"),
                Role = "Customer",
                Balance = 500
            };

            context.Users.AddRange(admin, customer);
            context.SaveChanges();
        }
    }
}
