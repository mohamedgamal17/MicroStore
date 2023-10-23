using MicroStore.Shipping.Application.Abstraction.Common;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using MicroStore.Shipping.Application.Abstraction.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace MicroStore.Shipping.Infrastructure
{
    [ExposeServices(typeof(IShipmentSystemResolver),IncludeDefaults = true,IncludeSelf = true)]
    public class ShipmentProviderResolver : IShipmentSystemResolver , ITransientDependency
    {

        private readonly List<ShippingSystem> _systems;

        private readonly IServiceProvider _serviceProvider;

        public ShipmentProviderResolver(IOptions<ShippingSystemOptions> options, IServiceProvider serviceProvider)
        {
            _systems = options.Value.Systems;
            _serviceProvider = serviceProvider;
        }

        public async Task<Result<IShipmentSystemProvider>> Resolve(string systemName, CancellationToken cancellationToken = default)
        {
            var system =  _systems.SingleOrDefault(x => x.Name == systemName);

            if (system == null)
            {
                return new Result<IShipmentSystemProvider>(new EntityNotFoundException($"Shipping system with name: {systemName} is not exist"));
            }

            if (!system.IsEnabled)
            {
                return new Result<IShipmentSystemProvider>(new UserFriendlyException($"Shipping system with name {systemName} is not enabled"));
            }


            var provider = (IShipmentSystemProvider) _serviceProvider.GetRequiredService(system.Provider);

            return new Result<IShipmentSystemProvider>(provider);
        }
    }
}
