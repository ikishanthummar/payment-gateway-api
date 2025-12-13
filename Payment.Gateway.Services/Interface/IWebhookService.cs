using Payment.Gateway.DTOs.Payments;

namespace Payment.Gateway.Services.Interface
{
    public interface IWebhookService
    {
        Task ProcessPaymentCallbackAsync(PaymentCallbackRequest callback, string rawPayload, string signature);
    }
}
