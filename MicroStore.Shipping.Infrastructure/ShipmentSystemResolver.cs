using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.Domain.Repositories;
using MicroStore.BuildingBlocks.Results.Http;

namespace MicroStore.Shipping.Infrastructure
{
    public class ShipmentSystemResolver : IShipmentSystemResolver, ISingletonDependency
    {
        private readonly IEnumerable<IShipmentSystemProvider> _providers;

        private readonly IRepository<ShippingSystem> _shippingSystemRepository;
        public ShipmentSystemResolver(IEnumerable<IShipmentSystemProvider> providers, IRepository<ShippingSystem> shippingSystemRepository)
        {
            _providers = providers;
            _shippingSystemRepository = shippingSystemRepository;
        }

        public async Task<UnitResult<IShipmentSystemProvider>> Resolve(string systemName, CancellationToken cancellationToken = default)
        {

            var system = await _shippingSystemRepository.SingleOrDefaultAsync(x=> x.Name == systemName);

            if(system == null)
            {
                return UnitResult.Failure<IShipmentSystemProvider>(ErrorInfo.NotFound($"Shipping system with name: { systemName} is not exist"));
            }

            if (!system.IsEnabled)
            {
                return UnitResult.Failure<IShipmentSystemProvider>(ErrorInfo.BusinessLogic($"Shipping system with name {systemName} is not enabled"));
            }

            var provider = _providers
                .Single(x => x.SystemName == systemName);


            return UnitResult.Success(provider);
        }
     
    }
}
