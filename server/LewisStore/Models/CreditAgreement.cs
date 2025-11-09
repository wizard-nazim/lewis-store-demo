namespace LewisStore.Models;

// Haram Credit Agreement for financing orders
public class CreditAgreement
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public decimal Principal { get; set; }
    public decimal InterestRateAnnual { get; set; }
    public int TermMonths { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public decimal OutstandingBalance { get; set; }
    public string Status { get; set; } = "Active";
}
