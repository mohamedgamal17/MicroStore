using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
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

        public async Task<ResultV2<IShipmentSystemProvider>> Resolve(string systemName, CancellationToken cancellationToken = default)
        {

            var system = await _shippingSystemRepository.SingleOrDefaultAsync(x=> x.Name == systemName);

            if(system == null)
            {
                return new ResultV2<IShipmentSystemProvider>( new EntityNotFoundException( $"Shipping system with name: { systemName} is not exist"));
            }

            if (!system.IsEnabled)
            {
                return new ResultV2<IShipmentSystemProvider>(new BusinessException($"Shipping system with name {systemName} is not enabled"));
            }

            var provider = _providers
                .Single(x => x.SystemName == systemName);


            return new ResultV2<IShipmentSystemProvider>(provider);
        }
     
    }
}
