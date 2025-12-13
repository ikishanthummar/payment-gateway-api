using System.ComponentModel.DataAnnotations;

namespace Payment.Gateway.Data.Entities
{
    public class WebhookLog
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid TransactionId { get; set; }
        
        public string Payload { get; set; } = string.Empty;
        
        public string Signature { get; set; } = string.Empty;

        public bool IsVerified { get; set; }

        public string ProcessingStatus { get; set; } = "Received";

        public string? ErrorMessage { get; set; }

        public DateTime ReceivedOn { get; set; } = DateTime.UtcNow;

        public virtual Transaction Transaction { get; set; } = null!;
    }
}
