using Payment.Gateway.DTOs.Payments;

namespace Payment.Gateway.Services.Interface
{
    public interface IWebhookService
    {
        Task ProcessPaymentCallbackAsync(PaymentCallbackDto callback, string rawPayload, string signature);
    }
}
