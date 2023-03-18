using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.Shipments
{
    public interface IShipmentQueryService : IApplicationService
    {
        Task<ResultV2<PagedResult<ShipmentListDto>>> ListAsync(PagingQueryParams queryParams,string? userId  = null,CancellationToken cancellationToken = default);
        Task<ResultV2<ShipmentDto>> GetAsync(string shipmentId, CancellationToken cancellationToken = default);
        Task<ResultV2 <ShipmentDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        Task<ResultV2<ShipmentDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    }
}
