using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Inventory.IntegrationEvents.Models;
using MicroStore.Ordering.Events;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderConfirmedActivity : IStateMachineActivity<OrderStateEntity, OrderConfirmedEvent>, ITransientDependency
    {
        private readonly ILogger<OrderConfirmedActivity> _logger;

        public OrderConfirmedActivity(ILogger<OrderConfirmedActivity> logger)
        {
            _logger = logger;
        }


        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Task Execute(BehaviorContext<OrderStateEntity, OrderConfirmedEvent> context, IBehavior<OrderStateEntity, OrderConfirmedEvent> next)
        {
            _logger.LogDebug("Executing Order Confirmed Activity");

            ConfirmStockIntegrationEvent integrationEvent = new ConfirmStockIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId,
                OrderNumber = context.Saga.OrderNumber,
                Products = MapStockItems(context.Saga.OrderItems)
            };

            return context.Publish(integrationEvent);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderConfirmedEvent, TException> context, IBehavior<OrderStateEntity, OrderConfirmedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("confirmed-order");
        }


        private List<ProductModel> MapStockItems(List<OrderItemEntity> stockItems)
        {
            return stockItems.Select(x => new ProductModel
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList();
        }
    }
}
