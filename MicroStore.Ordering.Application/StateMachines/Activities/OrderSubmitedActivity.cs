using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Inventory.IntegrationEvents.Models;
using MicroStore.Ordering.Events;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderSubmitedActivity : IStateMachineActivity<OrderStateEntity, OrderSubmitedEvent>, ITransientDependency
    {
        private readonly ILogger<OrderSubmitedActivity> _logger;

        public OrderSubmitedActivity(ILogger<OrderSubmitedActivity> logger)
        {
            _logger = logger;
        }


        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Task Execute(BehaviorContext<OrderStateEntity, OrderSubmitedEvent> context, IBehavior<OrderStateEntity, OrderSubmitedEvent> next)
        {
            _logger.LogDebug("Executing Order Confirmed Activity");

            AllocateOrderStockIntegrationEvent integrationEvent = new AllocateOrderStockIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId,
                OrderNumber = context.Saga.OrderNumber,
                Products = MapStockItems(context.Saga.OrderItems)
            };

            return context.Publish(integrationEvent);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderSubmitedEvent, TException> context, IBehavior<OrderStateEntity, OrderSubmitedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("submited-order");
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
