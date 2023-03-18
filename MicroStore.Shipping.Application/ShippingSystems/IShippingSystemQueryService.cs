using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.ShippingSystems
{
    public interface IShippingSystemQueryService : IApplicationService
    {
        Task<ResultV2<ShippingSystemDto>> GetAsync(string systemId, CancellationToken cancellationToken = default);
        Task<ResultV2<ShippingSystemDto>> GetByNameAsync(string systemName, CancellationToken cancellationToken = default);
        Task<ResultV2<List<ShippingSystemDto>>> ListAsync(CancellationToken cancellationToken = default);
    }
}
