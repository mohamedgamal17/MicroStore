using MicroStore.Ordering.Application.Abstractions.StateMachines;

namespace MicroStore.Ordering.Application.Abstractions.Common
{
    public interface IOrderRepository
    {
        Task<OrderStateEntity?> GetOrder(Guid orderId);

    }
}
