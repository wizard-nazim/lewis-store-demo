namespace LewisStore.Services;

public interface IPaymentService
{
    // Simulate creating a payment session (redirect URL etc)
    Task<string> CreatePaymentSessionAsync(Guid userId, decimal amount, string callbackUrl);
    // Simulate webhook: provider calls your webhook endpoint
    Task<bool> SimulateWebhookPaymentSucceededAsync(Guid sessionId);
}
