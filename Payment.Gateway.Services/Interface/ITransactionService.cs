using Payment.Gateway.Data.Entities;
using Payment.Gateway.DTOs;
using Payment.Gateway.DTOs.Common;

namespace Payment.Gateway.Services.Interface
{
    public interface ITransactionService
    {
        Task<PagedResult<TransactionView>> GetTransactionsAsync(TransactionListRequestDto request);

        Task<Transaction> GetTransactionByTNumberAsync(string transactionNumber);

        Task<Transaction> GetTransactionByIdAsync(Guid id);

        Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId);
    }
}
