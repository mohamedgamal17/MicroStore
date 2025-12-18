#nullable disable
using MicroStore;

namespace MicroStore.Payment.Domain.Shared.Events
{
    public class PaymentAcceptedEvent
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserName { get; set; }
        public string TransactionId { get; set; }
        public string PaymentGateway { get; set; }
    }
}
