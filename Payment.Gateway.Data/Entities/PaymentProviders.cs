using System.ComponentModel.DataAnnotations;

namespace Payment.Gateway.Data.Entities
{
    public class PaymentProviders
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
