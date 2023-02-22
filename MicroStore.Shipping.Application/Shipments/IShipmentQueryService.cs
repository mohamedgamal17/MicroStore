using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.Shipments
{
    public interface IShipmentQueryService : IApplicationService
    {
        Task<UnitResultV2<PagedResult<ShipmentListDto>>> ListAsync(PagingQueryParams queryParams,string? userId  = null,CancellationToken cancellationToken = default);
        Task<UnitResultV2<ShipmentDto>> GetAsync(string shipmentId, CancellationToken cancellationToken = default);
        Task<UnitResultV2<ShipmentDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        Task<UnitResultV2<ShipmentDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    }
}
