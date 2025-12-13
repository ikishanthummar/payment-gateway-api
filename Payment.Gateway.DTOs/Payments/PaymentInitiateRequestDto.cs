using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Gateway.DTOs.Payments
{
    public class PaymentInitiateRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [MaxLength(200)]
        public string ProviderReference { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
