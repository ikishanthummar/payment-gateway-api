using Payment.Gateway.DTOs.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Gateway.DTOs
{
    public class CreateTransactionReq
    {
        [MaxLength(200)]
        public string ProviderReference { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }

    public class TransactionListRequestDto : PageRequest
    {
        
    }

    public class TransactionView
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string TransactionNumber { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string ProviderReference { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
