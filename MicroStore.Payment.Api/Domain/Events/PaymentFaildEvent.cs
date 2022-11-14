#nullable disable
namespace MicroStore.Payment.Api.Domain.Events
{
    public class PaymentFaildEvent
    {
        public Guid PaymentId { get; init; }
        public Guid OrderId { get; init; }
        public string OrderNumber { get; init; }
        public string CustomerId { get; set; }
    }
}
