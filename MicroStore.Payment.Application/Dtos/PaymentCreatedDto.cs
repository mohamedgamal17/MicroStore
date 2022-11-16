#nullable disable
namespace MicroStore.Payment.Application.Dtos
{
    public class PaymentCreatedDto
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
