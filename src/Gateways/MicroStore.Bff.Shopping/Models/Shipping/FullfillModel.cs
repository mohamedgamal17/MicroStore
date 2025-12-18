using MicroStore.Bff.Shopping.Data.Common;
using MicroStore.Bff.Shopping.Models.Common;

namespace MicroStore.Bff.Shopping.Models.Shipping
{
    public class FullfillModel
    {
        public WeightModel Weight { get; set; }
        public DimensionModel Dimensions { get; set; }

        public FullfillModel()
        {
            Weight = new WeightModel();
            Dimensions = new DimensionModel();
        }
    }
}
