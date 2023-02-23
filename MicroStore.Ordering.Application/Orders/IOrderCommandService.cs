using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Ordering.Application.Orders
{
    public interface IOrderCommandService : IApplicationService
    {
        Task<UnitResult<OrderSubmitedDto>> CreateOrderAsync(CreateOrderModel model, CancellationToken cancellationToken = default);
        Task<UnitResult> FullfillOrderAsync(Guid orderId,FullfillOrderModel model , CancellationToken cancellationToken = default);
        Task<UnitResult> CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<UnitResult> CancelOrderAsync(Guid orderId, CancelOrderModel model, CancellationToken cancellationToken = default);

    }
}
