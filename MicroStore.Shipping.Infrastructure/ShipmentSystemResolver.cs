using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Const;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Shipping.Infrastructure
{
    public class ShipmentSystemResolver : IShipmentSystemResolver, ISingletonDependency
    {
        private readonly IList<IShipmentSystemProvider> _providers;


        public ShipmentSystemResolver(IList<IShipmentSystemProvider> providers)
        {
            _providers = providers;
        }

        public async Task<AggregateEstimatedRateCollection> AggregateEstimationRate(EstimatedRateModel estimatedRateModel, CancellationToken cancellationToken = default)
        {
            AggregateEstimatedRateCollection estimatedRates = new AggregateEstimatedRateCollection();

            foreach (var provider in _providers)
            {
                var systemRates = new AggregateEstimatedRateDto
                {
                    SystemName = provider.SystemName,
                    Rates = await provider.EstimateShipmentRate(estimatedRateModel)
                };

                estimatedRates.Add(systemRates);
            }

            return estimatedRates;
        }

        public async Task<UnitResult<IShipmentSystemProvider>> Resolve(string systemName, CancellationToken cancellationToken = default)
        {
            var provider = _providers
                .SingleOrDefault(x => x.SystemName == systemName);

            if (provider == null)
            {
                return UnitResult.Failure<IShipmentSystemProvider>(ShippingSystemErrorType.NotExist, "Shipping system with name: { systemName} is not exist");
            }

            bool isActive = await provider.IsActive(cancellationToken);

            if (!isActive)
            {

                return UnitResult.Failure<IShipmentSystemProvider>(ShippingSystemErrorType.BusinessLogicError, $"shipping system : {systemName} is not active");
            }


            return UnitResult.Success(provider);
        }
    }
}
