using Microsoft.Extensions.Options;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Infrastructure.DimensionProviders
{
    public class CentiMeterSystemProvider : IDimensionSystemProvider
    {
        public string SystemName => DimensionProviderConsts.CentiMeterSystem;

        private readonly DimensionSystemConfig _config;

        public CentiMeterSystemProvider(IOptions<DimensionSystemConfig> options)
        {
            _config = options.Value;
        }


        public Dimension EstimateDimension(List<DimensionModel> dimensionModels)
        {
            var simplifiedDimensions = dimensionModels.Select(x => x.AsDimension())
                .Where(x => !x.IsEmpty)
                .ToList();

            var convertedDimensions = simplifiedDimensions
                .Select(x => Dimension.ConvertToCentiMeter(x))
                .ToList();

            var aggregatedDimension = convertedDimensions.Aggregate((curr, nex) => Dimension.Estimate(curr, nex));

            double estimatedWidth = aggregatedDimension.Width * _config.EstimationPercentage + aggregatedDimension.Width;

            double estimatdLength = aggregatedDimension.Length * _config.EstimationPercentage + aggregatedDimension.Length;

            double estimatedHeight = aggregatedDimension.Height * _config.EstimationPercentage + aggregatedDimension.Height;

            return Dimension.FromCentiMeter(estimatedWidth, estimatdLength, estimatedHeight);
        }
    }
}
