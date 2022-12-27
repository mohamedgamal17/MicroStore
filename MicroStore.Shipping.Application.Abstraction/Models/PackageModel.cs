using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class PackageModel
    {
        public WeightModel Weight { get; set; }
        public DimensionModel Dimension { get; set; }
    }
}
