#nullable disable
using MicroStore;

namespace MicroStore.Payment.Application.Domain.Events
{
    public class PaymentCompletedEvent
    {
        public Guid PaymentId { get; init; }
        public Guid OrderId { get; init; }
        public string OrderNumber { get; init; }
        public DateTime CapturedAt { get; set; }

    }
}
