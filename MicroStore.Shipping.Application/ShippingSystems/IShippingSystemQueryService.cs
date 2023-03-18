using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.ShippingSystems
{
    public interface IShippingSystemQueryService : IApplicationService
    {
        Task<Result<ShippingSystemDto>> GetAsync(string systemId, CancellationToken cancellationToken = default);
        Task<Result<ShippingSystemDto>> GetByNameAsync(string systemName, CancellationToken cancellationToken = default);
        Task<Result<List<ShippingSystemDto>>> ListAsync(CancellationToken cancellationToken = default);
    }
}
