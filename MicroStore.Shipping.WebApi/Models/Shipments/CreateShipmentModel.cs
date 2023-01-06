using MicroStore.Shipping.Application.Abstraction.Models;

namespace MicroStore.Shipping.WebApi.Models.Shipments
{
    public class CreateShipmentModel
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public AddressModel Address { get; set; }
        public List<ShipmentItemModel> Items { get; set; }
    }
}
