#nullable disable
using MicroStore;

namespace MicroStore.Payment.Application.Domain.Events
{
    public class PaymentCreatedEvent
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }

    }
}
