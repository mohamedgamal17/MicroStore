using MicroStore.Bff.Shopping.Data.Common;
using MicroStore.Bff.Shopping.Models.Common;
namespace MicroStore.Bff.Shopping.Models.Shipping
{
    public class EstimateRateModel
    {
        public AddressModel Address { get; set; }

        public List<EstimatedRateItemModel> Items { get; set; }
    }
    public class EstimatedRateItemModel
    {
        public string name { get; set; }
        public string sku { get; set; } 
        public Money UnitPrice { get; set; }
        public int Quantity { get; set; }
        public WeightModel Weight { get; set; }
    }
}
