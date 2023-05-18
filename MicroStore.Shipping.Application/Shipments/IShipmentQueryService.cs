using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.Shipments
{
    public interface IShipmentQueryService : IApplicationService
    {
        Task<Result<PagedResult<ShipmentListDto>>> ListAsync(PagingQueryParams queryParams,string? userId  = null,CancellationToken cancellationToken = default);
        Task<Result<ShipmentDto>> GetAsync(string shipmentId, CancellationToken cancellationToken = default);
        Task<Result <ShipmentDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        Task<Result<ShipmentDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<ShipmentListDto>>> SearchByOrderNumber(ShipmentSearchByOrderNumberModel model, CancellationToken cancellationToken = default);
    }
}
