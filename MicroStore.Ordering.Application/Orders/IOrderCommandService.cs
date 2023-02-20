using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Ordering.Application.Orders
{
    public interface IOrderCommandService : IApplicationService
    {
        Task<UnitResultV2<OrderSubmitedDto>> CreateOrderAsync(CreateOrderModel model, CancellationToken cancellationToken = default);
        Task<UnitResultV2> FullfillOrderAsync(Guid orderId,FullfillOrderModel model , CancellationToken cancellationToken = default);
        Task<UnitResultV2> CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<UnitResultV2> CancelOrderAsync(Guid orderId, CancelOrderModel model, CancellationToken cancellationToken = default);

    }
}
