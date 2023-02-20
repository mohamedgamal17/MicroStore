using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using Volo.Abp.Application.Services;
namespace MicroStore.Ordering.Application.Orders
{

    public interface IOrderQueryService : IApplicationService
    {
        Task<UnitResultV2<PagedResult<OrderListDto>>> ListAsync(PagingAndSortingQueryParams queryParams, string? userId = null , CancellationToken cancellationToken = default);
        Task<UnitResultV2<OrderDto>> GetAsync(Guid orderId,  CancellationToken cancellationToken =default);
        Task<UnitResultV2<OrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    }
}
