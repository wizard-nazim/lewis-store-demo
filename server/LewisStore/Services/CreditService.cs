using LewisStore.Data;
using LewisStore.Models;

namespace LewisStore.Services;

// Service to handle credit agreements and installment creation but it's also haram
public class CreditService : ICreditService
{
    private readonly LewisDbContext _db;
    public CreditService(LewisDbContext db) => _db = db;

    public async Task<CreditAgreement> CreateAgreementAsync(Guid orderId, decimal principal, int termMonths, decimal annualInterestRate)
    {
        var agreement = new CreditAgreement
        {
            OrderId = orderId,
            Principal = principal,
            InterestRateAnnual = annualInterestRate,
            TermMonths = termMonths,
            OutstandingBalance = principal
        };
        _db.CreditAgreements.Add(agreement);
        await _db.SaveChangesAsync();

        var r = annualInterestRate / 12m;
        var P = principal;
        var n = termMonths;
        decimal monthly;
        if (r == 0) monthly = P / n;
        else
        {
            var rDouble = (double)r;
            var PDouble = (double)P;
            var nDouble = n;
            var m = (decimal)((rDouble * PDouble) / (1 - Math.Pow(1 + rDouble, -nDouble)));
            monthly = Math.Round(m, 2);
        }

        var dueBase = DateTime.UtcNow.AddMonths(1);
        for (int i = 0; i < n; i++)
        {
            var interestComp = Math.Round(agreement.OutstandingBalance * r, 2);
            var principalComp = Math.Round(monthly - interestComp, 2);
            if (i == n - 1) principalComp = agreement.OutstandingBalance;
            var inst = new Installment
            {
                CreditAgreementId = agreement.Id,
                DueDate = dueBase.AddMonths(i),
                AmountDue = principalComp + interestComp,
                PrincipalComponent = principalComp,
                InterestComponent = interestComp
            };
            agreement.OutstandingBalance -= principalComp;
            _db.Installments.Add(inst);
        }

        await _db.SaveChangesAsync();
        return agreement;
    }
}
