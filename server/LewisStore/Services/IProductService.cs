using LewisStore.Models;

namespace LewisStore.Services;

public interface IProductService
{
    Task<Product[]> GetAllAsync(string? search = null);
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(Product p);
    Task<Product?> UpdateAsync(Guid id, Product p);
    Task<bool> ArchiveAsync(Guid id);
}
