namespace LewisStore.Models;

public class Installment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CreditAgreementId { get; set; }
    public CreditAgreement? CreditAgreement { get; set; }
    public DateTime DueDate { get; set; }
    public decimal AmountDue { get; set; }
    public decimal PrincipalComponent { get; set; }
    public decimal InterestComponent { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public string Status { get; set; } = "Pending";
}
