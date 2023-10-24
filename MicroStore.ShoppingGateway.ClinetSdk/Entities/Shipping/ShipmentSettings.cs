using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping
{
    public class ShipmentSettings
    {
        public string DefaultShippingSystem { get; set; }

        public Address Location { get; set; }
    }
}
