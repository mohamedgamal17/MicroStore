using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Shipping.Application.ShippingSystems
{
    public class ShippingSystemCommandService : ShippingApplicationService, IShippingSystemCommandService
    {
        private readonly IRepository<ShippingSystem> _shippingSystemRepository;

        public ShippingSystemCommandService(IRepository<ShippingSystem> shippingSystemRepository)
        {
            _shippingSystemRepository = shippingSystemRepository;
        }

        public async Task<UnitResultV2<ShippingSystemDto>> EnableAsync(string systemName, bool isEnabled, CancellationToken cancellationToken = default)
        {
            var system = await _shippingSystemRepository.SingleOrDefaultAsync(x => x.Name == systemName,cancellationToken);

            if (system == null)
            {
                return UnitResultV2.Failure<ShippingSystemDto>(ErrorInfo.NotFound($"Shipping system with name : {systemName} is not exist"));

            }


            system.IsEnabled = isEnabled;

            await _shippingSystemRepository.UpdateAsync(system);


            return UnitResultV2.Success(ObjectMapper.Map<ShippingSystem, ShippingSystemDto>(system));
        }
    }
}
