using Microsoft.EntityFrameworkCore;
using LewisStore.Data;
using LewisStore.Models;

namespace LewisStore.Services;

public class ProductService : IProductService
{
    private readonly LewisDbContext _db;
    public ProductService(LewisDbContext db) => _db = db;

    public async Task<Product[]> GetAllAsync(string? search = null)
    {
        var q = _db.Products.AsQueryable().Where(p => p.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(p => p.Name.Contains(search) || p.SKU.Contains(search));
        return await q.OrderBy(p => p.Name).ToArrayAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id) => await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

    public async Task<Product> CreateAsync(Product p)
    {
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        return p;
    }

    public async Task<Product?> UpdateAsync(Guid id, Product p)
    {
        var x = await _db.Products.FindAsync(id);
        if (x == null) return null;
        x.Name = p.Name;
        x.SKU = p.SKU;
        x.UnitPrice = p.UnitPrice;
        x.StockQty = p.StockQty;
        x.IsActive = p.IsActive;
        await _db.SaveChangesAsync();
        return x;
    }

    public async Task<bool> ArchiveAsync(Guid id)
    {
        var x = await _db.Products.FindAsync(id);
        if (x == null) return false;
        x.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}
