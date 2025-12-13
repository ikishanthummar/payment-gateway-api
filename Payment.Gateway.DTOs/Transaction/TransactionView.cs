namespace Payment.Gateway.DTOs.Transaction
{
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
