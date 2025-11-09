using LewisStore.Models;

namespace LewisStore.Services;

public interface ICreditService
{
    Task<CreditAgreement> CreateAgreementAsync(Guid orderId, decimal principal, int termMonths, decimal annualInterestRate);
}
