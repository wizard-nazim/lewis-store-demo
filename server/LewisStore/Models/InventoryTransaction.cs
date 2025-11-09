namespace LewisStore.Models;

public class InventoryTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int ChangeQty { get; set; }
    public string Type { get; set; } = "Adjustment";
    public string? Note { get; set; }
    public Guid? PerformedBy { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
}
