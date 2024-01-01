using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Ordering.Application.Orders
{
    public interface IOrderCommandService : IApplicationService
    {
        Task<Result<OrderSubmitedDto>> CreateOrderAsync(CreateOrderModel model, CancellationToken cancellationToken = default);
        Task<Result<Unit>> FullfillOrderAsync(Guid orderId,FullfillOrderModel model , CancellationToken cancellationToken = default);
        Task<Result<Unit>> CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<Result<Unit>> CancelOrderAsync(Guid orderId, CancelOrderModel model, CancellationToken cancellationToken = default);

    }
}
