using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Data
{
    public class ShippingSystemDataSeederContributor : IDataSeedContributor , ITransientDependency
    {
        private readonly IRepository<ShippingSystem> _shippingSystemRepository;

        public ShippingSystemDataSeederContributor(IRepository<ShippingSystem> shippingSystemRepository)
        {
            _shippingSystemRepository = shippingSystemRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            bool isExist = await _shippingSystemRepository.AnyAsync(x => x.Name == ShipEngineConst.SystemName);

            if (isExist)
            {
                return;
            }


            await _shippingSystemRepository.InsertAsync(new ShippingSystem
            {
                Name = ShipEngineConst.SystemName,
                DisplayName = ShipEngineConst.DisplayName,
                IsEnabled = false,
                Image = "NONE"
            });
        }
    }
}
