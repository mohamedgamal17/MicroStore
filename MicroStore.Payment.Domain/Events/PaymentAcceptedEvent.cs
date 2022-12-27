#nullable disable
using MicroStore;

namespace MicroStore.Payment.Domain.Events
{
    public class PaymentAcceptedEvent
    {
        public Guid PaymentId { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentGateway { get; set; }
    }
}
