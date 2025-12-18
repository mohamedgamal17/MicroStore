using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Inventory.IntegrationEvents.Models;
using MicroStore.Ordering.Application.Domain;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderAcceptedActivity : IStateMachineActivity<OrderStateEntity, OrderPaymentAcceptedEvent>, ITransientDependency
    {
        private readonly ILogger<OrderAcceptedActivity> _logger;

        public OrderAcceptedActivity(ILogger<OrderAcceptedActivity> logger)
        {
            _logger = logger;
        }


        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Task Execute(BehaviorContext<OrderStateEntity, OrderPaymentAcceptedEvent> context, IBehavior<OrderStateEntity, OrderPaymentAcceptedEvent> next)
        {
            _logger.LogDebug("Executing Order Confirmed Activity");

            AllocateOrderStockIntegrationEvent integrationEvent = new AllocateOrderStockIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId.ToString(),
                OrderNumber = context.Saga.OrderNumber,
                UserId = context.Saga.UserId,
                Items = MapStockItems(context.Saga.OrderItems)
            };

            return context.Publish(integrationEvent);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderPaymentAcceptedEvent, TException> context, IBehavior<OrderStateEntity, OrderPaymentAcceptedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("submited-order");
        }


        private List<OrderItemModel> MapStockItems(List<OrderItemEntity> stockItems)
        {
            return stockItems.Select(x => new OrderItemModel
            {
                ProductId = x.ExternalProductId.ToString(),
                Quantity = x.Quantity
            }).ToList();
        }

    }
}
