using MicroStore.Bff.Shopping.Data.Common;

namespace MicroStore.Bff.Shopping.Data.Shipping
{
    public class ShipmentRate
    {
        public string CarrierId { get; set; }
        public Money Amount { get; set; }
        public ServiceLevel ServiceLevel { get; set; }
        public int? Days { get; set; }
    }
}
