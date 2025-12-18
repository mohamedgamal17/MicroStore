using MicroStore.Shipping.IntegrationEvents.Models;

namespace MicroStore.Shipping.IntegrationEvents
{
    public class CreateShipmentIntegrationEvent
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public AddressModel Address { get; set; }
        public List<ShipmentItemModel> Items { get; set; }

    }
}