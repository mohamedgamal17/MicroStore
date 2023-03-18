using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Ordering.Application.Orders
{
    public interface IOrderCommandService : IApplicationService
    {
        Task<ResultV2<OrderSubmitedDto>> CreateOrderAsync(CreateOrderModel model, CancellationToken cancellationToken = default);
        Task<ResultV2<Unit>> FullfillOrderAsync(Guid orderId,FullfillOrderModel model , CancellationToken cancellationToken = default);
        Task<ResultV2<Unit>> CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<ResultV2<Unit>> CancelOrderAsync(Guid orderId, CancelOrderModel model, CancellationToken cancellationToken = default);

    }
}
