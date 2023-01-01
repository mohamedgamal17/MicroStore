using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Const;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Shipping.Infrastructure
{
    public class ShipmentSystemResolver : IShipmentSystemResolver, ISingletonDependency
    {
        private readonly IList<IShipmentSystemProvider> _providers;

        private readonly IRepository<ShippingSystem> _shippingSystemRepository;
        public ShipmentSystemResolver(IList<IShipmentSystemProvider> providers, IRepository<ShippingSystem> shippingSystemRepository)
        {
            _providers = providers;
            _shippingSystemRepository = shippingSystemRepository;
        }

        public async Task<UnitResult<IShipmentSystemProvider>> Resolve(string systemName, CancellationToken cancellationToken = default)
        {

            var system = await _shippingSystemRepository.SingleOrDefaultAsync(x=> x.Name == systemName);

            if(system == null)
            {
                return UnitResult.Failure<IShipmentSystemProvider>(ShippingSystemErrorType.NotExist, $"Shipping system with name: { systemName} is not exist");
            }

            if (!system.IsEnabled)
            {
                return UnitResult.Failure<IShipmentSystemProvider>(ShippingSystemErrorType.BusinessLogicError, $"Shipping system with name {systemName} is not enabled");
            }

            var provider = _providers
                .Single(x => x.SystemName == systemName);


            return UnitResult.Success(provider);
        }
     
    }
}
