using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using Volo.Abp.Application.Services;
namespace MicroStore.Ordering.Application.Orders
{

    public interface IOrderQueryService : IApplicationService
    {
        Task<ResultV2<PagedResult<OrderListDto>>> ListAsync(PagingAndSortingQueryParams queryParams, string? userId = null , CancellationToken cancellationToken = default);
        Task<ResultV2<OrderDto>> GetAsync(Guid orderId,  CancellationToken cancellationToken =default);
        Task<ResultV2<OrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    }
}
