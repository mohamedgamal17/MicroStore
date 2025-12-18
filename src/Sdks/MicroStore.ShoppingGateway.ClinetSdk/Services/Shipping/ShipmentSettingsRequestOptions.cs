using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentSettingsRequestOptions
    {
        public string DefaultShippingSystem { get; set; }
        public Address Location { get; set; }
    }
}
