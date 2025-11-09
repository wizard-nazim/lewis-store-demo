using System.ComponentModel.DataAnnotations;

namespace LewisStore.Models;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required] public string Name { get; set; } = "";
    public string SKU { get; set; } = "";
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal CostPrice { get; set; }
    public decimal? Weight { get; set; }
    public int StockQty { get; set; }
    public int ReorderThreshold { get; set; }
    public bool IsActive { get; set; } = true;
}
