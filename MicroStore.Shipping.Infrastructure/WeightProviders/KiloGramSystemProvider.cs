using Microsoft.Extensions.Options;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Infrastructure.WeightProviders
{
    public class KiloGramSystemProvider : IWeightSytemProvider
    {
        public string SystemName => WeightProviderConsts.KiloGramSystem;

        private readonly WeightSystemConfig _config;

        public KiloGramSystemProvider(IOptions<WeightSystemConfig> options)
        {
            _config = options.Value;
        }

        public Weight EstaimtedWeight(List<WeightModel> weights)
        {
            var simpilfiedWeights = weights.Select(x => x.AsWeight())
                .Where(x => x.IsEmpty)
                .ToList();

            var convertedWeights = simpilfiedWeights
                .Select(x => Weight.ConvertToKiloGram(x))
                .ToList();

            Weight totalWeight = convertedWeights.Aggregate((current, next) => current + next);

            Weight estimatedWeight = Weight.FromKiloGram(totalWeight.Value + (totalWeight.Value * _config.EstimationPercentage));

            return estimatedWeight;
        }


    }
}
