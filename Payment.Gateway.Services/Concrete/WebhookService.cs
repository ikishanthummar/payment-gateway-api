using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Data;
using Payment.Gateway.Data.Entities;
using Payment.Gateway.DTOs.Payments;
using Payment.Gateway.Services.Interface;

namespace Payment.Gateway.Services.Concrete
{
    public class WebhookService : IWebhookService
    {
        private readonly AppDBContext _db;

        public WebhookService(AppDBContext db)
        {
            _db = db;
        }

        public async Task ProcessPaymentCallbackAsync(PaymentCallbackDto callback, string rawPayload, string signature)
        {
            var transaction = await _db.Transactions.FirstOrDefaultAsync(t => t.TransactionNumber == callback.TransactionNumber);

            if (transaction == null)
                throw new KeyNotFoundException("Transaction not found");

            var alreadyProcessed = await _db.WebhookLogs.AnyAsync(w => w.TransactionId == transaction.Id && w.ProcessingStatus == "Processed");

            if (alreadyProcessed)
                return;

            using var dbTx = await _db.Database.BeginTransactionAsync();

            try
            {
                var log = new WebhookLog
                {
                    Id = Guid.NewGuid(),
                    TransactionId = transaction.Id,
                    Payload = rawPayload,
                    Signature = signature,
                    IsVerified = true,
                    ProcessingStatus = "Processed",
                    ReceivedOn = DateTime.UtcNow
                };

                _db.WebhookLogs.Add(log);

                transaction.Status = callback.Status.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase) ? "Success" : "Failed";

                transaction.UpdatedOn = DateTime.UtcNow;

                await _db.SaveChangesAsync();
                await dbTx.CommitAsync();
            }
            catch
            {
                await dbTx.RollbackAsync();
                throw;
            }
        }
    }
}
