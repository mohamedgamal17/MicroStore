using MicroStore.Inventory.IntegrationEvents.Models;

namespace MicroStore.Inventory.IntegrationEvents
{
    public class ReleaseOrderStockIntegrationEvent
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string PaymentId { get; set; }
        public string UserId { get; set; }

    }
}
