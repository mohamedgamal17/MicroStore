#nullable disable
using MicroStore;

namespace MicroStore.Payment.Domain.Shared.Events
{
    public class PaymentVoidedEvent
    {
        public Guid PaymentId { get; set; }
        public string TransactionId { get; set; }
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public DateTime FaultDate { get; set; }
        public string PaymentGatewayApi { get; set; }
    }
}
