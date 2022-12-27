using MicroStore.Inventory.IntegrationEvents.Models;

namespace MicroStore.Inventory.IntegrationEvents
{
    public class ReturnOrderStockIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public List<OrderItemModel> Products { get; set; }
    }
}
