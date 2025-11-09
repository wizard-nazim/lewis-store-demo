namespace LewisStore.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public User? Customer { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal Subtotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public string PaymentType { get; set; } = "Cash"; // Cash | Credit
    public string Status { get; set; } = "Pending";
    public ICollection<OrderItem>? Items { get; set; }
}
