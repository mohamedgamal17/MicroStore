using Microsoft.Extensions.Options;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Infrastructure.WeightProviders
{
    public class PoundSystemProvider : IWeightSytemProvider
    {
        public string SystemName => WeightProviderConsts.PoundSystem;

        private readonly WeightSystemConfig _config;

        public PoundSystemProvider(IOptions<WeightSystemConfig> options)
        {
            _config = options.Value;
        }

        public Weight EstaimtedWeight(List<WeightModel> weights)
        {
            var simpilfiedWeights = weights.Select(x => x.AsWeight())
               .Where(x => !x.IsEmpty)
               .ToList();

            var convertedWeights = simpilfiedWeights
                .Select(x => Weight.ConvertToPound(x))
                .ToList();

            Weight totalWeight = convertedWeights.Aggregate((current, next) => current + next);

            Weight estimatedWeight = Weight.FromPound(totalWeight.Value + (totalWeight.Value * _config.EstimationPercentage));

            return estimatedWeight;
        }
    }
}
