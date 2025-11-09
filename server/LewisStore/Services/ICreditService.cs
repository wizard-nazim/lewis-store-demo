using LewisStore.Models;

namespace LewisStore.Services;

// Interface for credit service to handle credit agreements but it's also haram
public interface ICreditService
{
    Task<CreditAgreement> CreateAgreementAsync(Guid orderId, decimal principal, int termMonths, decimal annualInterestRate);
}
