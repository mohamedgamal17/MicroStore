using MicroStore.ShoppingGateway.ClinetSdk.Common;
namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping
{
    public class EstimatedRate
    {
        public string Name { get; set; }
        public Money Money { get; set; }
        public int EstaimatedDays { get; set; }
        public DateTime? ShippingDate { get; set; }
    }
}
