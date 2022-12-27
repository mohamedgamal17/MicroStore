using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IDimensionSystemProvider
    {
        string SystemName { get; }
        Dimension EstimateDimension(List<DimensionModel> dimensionModels);

    }
}
