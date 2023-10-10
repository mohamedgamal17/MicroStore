using MicroStore.Ordering.Application.Domain;

namespace MicroStore.Ordering.Application.Common
{
    public interface IOrderRepository
    {
        Task<OrderStateEntity?> GetOrder(Guid orderId);
    }
}
