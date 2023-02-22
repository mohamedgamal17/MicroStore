namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentModel
    {
        public string OrderId { get; set; }

        public string  OrderNumber { get; set; }
        public string UserId { get; set; }
        public AddressModel Address { get; set; }
        public List<ShipmentItemModel> Items { get; set; }
    }
}
