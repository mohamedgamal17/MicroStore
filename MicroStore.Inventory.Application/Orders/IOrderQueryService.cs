using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Orders
{
    public interface IOrderQueryService : IApplicationService
    {
        Task<ResultV2<OrderDto>> GetOrderAsync(string orderId , CancellationToken cancellationToken = default);
        Task<ResultV2<OrderDto>> GetOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<ResultV2<PagedResult<OrderListDto>>> ListOrderAsync(PagingQueryParams queryParams,string? userId = null ,CancellationToken cancellationToken = default);
    }


}
