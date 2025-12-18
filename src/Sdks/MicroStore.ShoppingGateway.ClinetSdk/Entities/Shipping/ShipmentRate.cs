using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping
{
    public class ShipmentRate : BaseEntity<string>      
    {
        public string CarrierId { get; set; }
        public Money Amount { get; set; }
        public ServiceLevel ServiceLevel { get; set; }
        public int? Days { get; set; }


    }
}
