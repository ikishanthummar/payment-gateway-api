using Payment.Gateway.DTOs.Payments;
using Payment.Gateway.DTOs.Transaction;

namespace Payment.Gateway.Services.Interface
{
    public interface IPaymentService
    {
        Task<TransactionView> InitiatePaymentAsync(PaymentInitiateRequest req);
    }
}
