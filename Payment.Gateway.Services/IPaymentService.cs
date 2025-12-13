using Payment.Gateway.Data.Entities;
using Payment.Gateway.DTOs;
using Payment.Gateway.DTOs.Common;

namespace Payment.Gateway.Services
{
    public interface IPaymentService
    {
        Task<Transaction> CreateTransactionAsync(CreateTransactionReq req);

        Task<Transaction> GetTransactionByTNumberAsync(string transactionNumber);

        Task<Transaction> GetTransactionByIdAsync(Guid id);

        Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId);

        Task<PagedResult<TransactionView>> GetTransactionsAsync(TransactionListRequestDto request);

        Task<Transaction> UpdateStatusAsync(Guid id, string status, string providerRef = null);

        // Optionally: Task ProcessProviderCallbackAsync(CallbackDto dto)
    }
}
