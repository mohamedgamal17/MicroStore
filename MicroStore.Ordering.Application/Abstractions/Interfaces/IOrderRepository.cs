using MicroStore.Ordering.Application.StateMachines;

namespace MicroStore.Ordering.Application.Abstractions.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderStateEntity?> GetOrder(Guid orderId);

    }
}
