using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.ShippingSystems
{
    public interface IShippingSystemCommandService : IApplicationService
    {
        Task<UnitResult<ShippingSystemDto>> EnableAsync(string systemName, bool isEnabled, CancellationToken cancellationToken = default);
    }
}
