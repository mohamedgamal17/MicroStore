using MicroStore.Inventory.IntegrationEvents.Models;

namespace MicroStore.Inventory.IntegrationEvents
{
    public class AllocateOrderStockIntegrationEvent
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }
}
