using Payment.Gateway.DTOs.Enum;

namespace Payment.Gateway.DTOs.Payments
{
    public class PaymentCallbackDto
    {
        public string TransactionNumber { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
    }
}
