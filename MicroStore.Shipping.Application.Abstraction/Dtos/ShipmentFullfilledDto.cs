namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class ShipmentFullfilledDto
    {
        public Guid ShipmentId { get; set; }
        public string ExternalShipmentId { get; set; }
        public string TrackingNumber { get; set; }
        public AddressDto AddressFrom { get; set; }
        public AddressDto AddressTo { get; set; }
        public List<ShipmentItemDto> Items { get; set; }
    }
}
