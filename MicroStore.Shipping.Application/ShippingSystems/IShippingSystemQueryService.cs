using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.ShippingSystems
{
    public interface IShippingSystemQueryService : IApplicationService
    {
        Task<UnitResult<ShippingSystemDto>> GetAsync(string systemId, CancellationToken cancellationToken = default);
        Task<UnitResult<ShippingSystemDto>> GetByNameAsync(string systemName, CancellationToken cancellationToken = default);
        Task<UnitResult<List<ShippingSystemDto>>> ListAsync(CancellationToken cancellationToken = default);
    }
}
