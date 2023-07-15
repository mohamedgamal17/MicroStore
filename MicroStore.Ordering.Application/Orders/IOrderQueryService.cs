using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Ordering.Application.Orders
{

    public interface IOrderQueryService : IApplicationService
    {
        Task<Result<PagedResult<OrderDto>>> ListAsync(OrderListQueryModel queryParams, string? userId = null, CancellationToken cancellationToken = default);
        Task<Result<OrderDto>> GetAsync(Guid orderId,  CancellationToken cancellationToken =default);
        Task<Result<OrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<OrderDto>>> SearchByOrderNumber(OrderSearchModel model,CancellationToken cancellationToken = default);
    }
}
