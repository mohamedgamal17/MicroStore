using MicroStore.Inventory.IntegrationEvents.Models;

namespace MicroStore.Inventory.IntegrationEvents
{
    public class AllocateOrderStockIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}
