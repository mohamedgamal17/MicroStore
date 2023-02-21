using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Orders
{
    public interface IOrderQueryService : IApplicationService
    {
        Task<UnitResultV2<OrderDto>> GetOrderAsync(string orderId , CancellationToken cancellationToken = default);
        Task<UnitResultV2<OrderDto>> GetOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<UnitResultV2<PagedResult<OrderListDto>>> ListOrderAsync(PagingQueryParams queryParams,string? userId = null ,CancellationToken cancellationToken = default);
    }


}
