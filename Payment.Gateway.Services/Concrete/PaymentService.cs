using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Data;
using Payment.Gateway.Data.Entities;
using Payment.Gateway.DTOs.Payments;
using Payment.Gateway.DTOs.Transaction;
using Payment.Gateway.Services.Interface;

namespace Payment.Gateway.Services.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDBContext _db;

        public PaymentService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<TransactionView> InitiatePaymentAsync(PaymentInitiateRequestDto req)
        {
            var transaction = new Transaction();

            transaction.Id = Guid.NewGuid();
            transaction.OrderId = req.OrderId;

            var count = await _db.Transactions.CountAsync();
            transaction.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{count + 1:D5}";

            string shortId = Guid.NewGuid().ToString("N")[..6].ToUpper();
            transaction.TransactionNumber = $"TXN-{DateTime.UtcNow:yyyyMMdd}-{shortId}";

            transaction.ProviderReference = req.ProviderReference;
            transaction.Amount = req.Amount;
            transaction.CreatedOn = DateTime.UtcNow;
            transaction.Status = "Pending";

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            var response = new TransactionView
            {
                Id = transaction.Id,
                OrderId = transaction.OrderId,
                TransactionNumber = transaction.TransactionNumber,
                OrderNumber = transaction.OrderNumber,
                ProviderReference = transaction.ProviderReference,
                Status = transaction.Status,
                Amount = transaction.Amount,
                CreatedOn = transaction.CreatedOn,
                UpdatedOn = transaction.UpdatedOn
            };

            return response;
        }
    }
}
