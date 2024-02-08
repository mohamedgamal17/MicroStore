using MicroStore.Bff.Shopping.Data.Common;
using MicroStore.Bff.Shopping.Models.Common;
namespace MicroStore.Bff.Shopping.Models.Shipping
{
    public class EstimateRateModel
    {
        public AddressModel Address { get; set; }

        public List<EstimatedRateItemModel> Items { get; set; }

        public EstimateRateModel()
        {
            Address = new AddressModel();
            Items = new List<EstimatedRateItemModel>();
        }
    }
    public class EstimatedRateItemModel
    {
        public string Name { get; set; }
        public string Sku { get; set; } 
        public Money UnitPrice { get; set; }
        public int Quantity { get; set; }
        public WeightModel Weight { get; set; }

        public EstimatedRateItemModel()
        {
            Name = string.Empty;
            Sku = string.Empty;
            UnitPrice = new Money();
            Quantity = 0;
            Weight = new WeightModel();
        }
    }
}
