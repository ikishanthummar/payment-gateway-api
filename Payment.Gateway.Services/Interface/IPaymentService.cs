using Payment.Gateway.Data.Entities;
using Payment.Gateway.DTOs;
using Payment.Gateway.DTOs.Payments;

namespace Payment.Gateway.Services.Interface
{
    public interface IPaymentService
    {
        Task<TransactionView> InitiatePaymentAsync(PaymentInitiateRequestDto req);


        Task<Transaction> UpdateStatusAsync(Guid id, string status, string providerRef = null);
    }
}
