using MicroStore.Ordering.Application.StateMachines;

namespace MicroStore.Ordering.Application.Common
{
    public interface IOrderRepository
    {
        Task<OrderStateEntity?> GetOrder(Guid orderId);

    }
}
