using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IWeightSytemProvider
    {
        string SystemName { get;}
        Weight EstaimtedWeight(List<WeightModel> weights);

    }
}
