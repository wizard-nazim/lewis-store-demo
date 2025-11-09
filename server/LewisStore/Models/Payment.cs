namespace LewisStore.Models;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? OrderId { get; set; }
    public Guid? CreditAgreementId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public string Method { get; set; } = "Manual";
    public string? Reference { get; set; }
}
