using Payment.Gateway.Data.Entities;
using Payment.Gateway.DTOs.Common;
using Payment.Gateway.DTOs.Transaction;

namespace Payment.Gateway.Services.Interface
{
    public interface ITransactionService
    {
        Task<PagedResult<TransactionView>> GetTransactionsAsync(TransactionListRequestDto request);

        Task<Transaction> GetTransactionByTNumberAsync(string transactionNumber);

        #region 
        //Task<Transaction> GetTransactionByIdAsync(Guid id);

        //Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId); 
        #endregion
    }
}
