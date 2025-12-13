namespace Payment.Gateway.DTOs
{
    public class ProviderCallbackDto
    {
        public string TransactionId { get; set; }   
        public string ProviderReference { get; set; }
        public string Status { get; set; }
    }
}
