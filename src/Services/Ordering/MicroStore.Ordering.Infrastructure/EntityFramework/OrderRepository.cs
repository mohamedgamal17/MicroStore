using Microsoft.EntityFrameworkCore;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Domain;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Ordering.Infrastructure.EntityFramework
{
    public class OrderRepository : IOrderRepository , ITransientDependency
    {

        private readonly OrderDbContext _orderDbContext;

        public OrderRepository(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public Task<OrderStateEntity?> GetOrder(Guid orderId)
        {
            return _orderDbContext.Set<OrderStateEntity>().SingleOrDefaultAsync(x => x.CorrelationId == orderId);
        }
    }
}
