using LewisStore.Data;
using LewisStore.Models;

namespace LewisStore.Services;

// Very simple in-memory session store for simulation
public class MockPaymentService : IPaymentService
{
    private readonly LewisDbContext _db;
    private static readonly Dictionary<Guid, (Guid userId, decimal amount)> _sessions = new();

    public MockPaymentService(LewisDbContext db) { _db = db; }

    public Task<string> CreatePaymentSessionAsync(Guid userId, decimal amount, string callbackUrl)
    {
        var sessionId = Guid.NewGuid();
        _sessions[sessionId] = (userId, amount);
        // return a fake session id string (client would redirect to a mock payment page)
        return Task.FromResult(sessionId.ToString());
    }

    // Simulate payment succeeded and call backend webhook handler (we will implement webhook controller that uses this)
    public Task<bool> SimulateWebhookPaymentSucceededAsync(Guid sessionId)
    {
        if (!_sessions.TryGetValue(sessionId, out var v)) return Task.FromResult(false);
        var (userId, amount) = v;
        var user = _db.Users.Find(userId);
        if (user == null) return Task.FromResult(false);
        user.Balance += amount;
        _db.Payments.Add(new Payment
        {
            Amount = amount,
            Method = "MockProvider",
            Reference = $"mock-session-{sessionId}",
            PaymentDate = DateTime.UtcNow
        });
        _db.SaveChanges();
        _sessions.Remove(sessionId);
        return Task.FromResult(true);
    }
}
