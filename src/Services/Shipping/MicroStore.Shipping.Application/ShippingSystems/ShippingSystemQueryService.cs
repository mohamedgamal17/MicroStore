using Microsoft.Extensions.Options;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Shipping.Application.Abstraction.Configuration;
using MicroStore.Shipping.Application.Abstraction.Dtos;
namespace MicroStore.Shipping.Application.ShippingSystems
{
    public class ShippingSystemQueryService : ShippingApplicationService, IShippingSystemQueryService
    {
        private readonly List<ShippingSystem> _systems;

        public ShippingSystemQueryService(IOptions<ShippingSystemOptions> options)
        {
            _systems = options.Value.Systems;
        }

        public async Task<Result<List<ShippingSystemDto>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var result = _systems.Select(x => new ShippingSystemDto
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                Image = x.Image,
                IsEnabled = x.IsEnabled
            }).ToList();


            return result;
        }
    }
}
