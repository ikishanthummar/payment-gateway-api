using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Gateway.Data.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        [MaxLength(100)]
        public string OrderNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string TransactionNumber { get; set; }

        [MaxLength(200)]
        public string ProviderReference { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<WebhookLog> WebhookLogs { get; set; } = new HashSet<WebhookLog>();
    }
}
