

using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Data;
using Payment.Gateway.Data.Entities;
using Payment.Gateway.DTOs;
using Payment.Gateway.DTOs.Common;
using Payment.Gateway.Services.Common;
using Payment.Gateway.Services.Interface;
using System.Linq.Expressions;

namespace Payment.Gateway.Services.Concrete
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDBContext _db;

        public TransactionService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<TransactionView>> GetTransactionsAsync(TransactionListRequestDto request)
        {
            var query = _db.Transactions.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.TransactionNumber.Contains(search) ||
                    x.OrderNumber.Contains(search) ||
                    x.ProviderReference.Contains(search) ||
                    x.Status.Contains(search));
            }

            var sortMap = new Dictionary<string, Expression<Func<Transaction, object>>>
            {
                ["amount"] = x => x.Amount,
                ["status"] = x => x.Status,
                ["createdOn"] = x => x.CreatedOn,
                ["updatedOn"] = x => x.CreatedOn
            };

            query = query.ApplySort(request.SortBy, request.IsDescending, sortMap, x => x.CreatedOn);

            var projectedQuery = query.Select(x => new TransactionView
            {
                Id = x.Id,
                OrderId = x.OrderId,
                TransactionNumber = x.TransactionNumber,
                OrderNumber = x.OrderNumber,
                ProviderReference = x.ProviderReference,
                Status = x.Status,
                Amount = x.Amount,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn
            });

            return await projectedQuery.ToPagedResultAsync(
                request.Page,
                request.PageSize);
        }

        public async Task<Transaction> GetTransactionByTNumberAsync(string transactionNumber)
        {
            var transaction = await _db.Transactions
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.TransactionNumber == transactionNumber);

            return transaction ?? throw new KeyNotFoundException($"Transaction not found for number: {transactionNumber}");
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            var transaction = await _db.Transactions
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Id == id);

            return transaction ?? throw new KeyNotFoundException($"Transaction not found for id: {id}");
        }

        public async Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId)
        {
            var transaction = await _db.Transactions
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.OrderId == orderId);

            return transaction ?? throw new KeyNotFoundException($"Transaction not found for order id: {orderId}");
        }
    }
}
