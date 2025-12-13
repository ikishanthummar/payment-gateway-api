namespace Payment.Gateway.DTOs.Payments
{
    public class PaymentCallbackDto
    {
        public string TransactionNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
