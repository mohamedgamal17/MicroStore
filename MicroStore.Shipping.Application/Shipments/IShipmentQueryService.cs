using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.Shipments
{
    public interface IShipmentQueryService : IApplicationService
    {
        Task<UnitResult<PagedResult<ShipmentListDto>>> ListAsync(PagingQueryParams queryParams,string? userId  = null,CancellationToken cancellationToken = default);
        Task<UnitResult<ShipmentDto>> GetAsync(string shipmentId, CancellationToken cancellationToken = default);
        Task<UnitResult <ShipmentDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        Task<UnitResult<ShipmentDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    }
}
