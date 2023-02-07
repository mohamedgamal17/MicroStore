using MassTransit;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.IntegrationEvents;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Ordering.Application.Consumers
{
    public class OrderOperationalEventConsumer : 
        IConsumer<SubmitOrderIntegrationEvent>,
        IConsumer<FullfillOrderIntegrationEvent>,
        IConsumer<CompleteOrderIntegrationEvent>,
        IConsumer<CancelOrderIntegrationEvent>
    {

        private readonly IObjectMapper _objectMapper;

        public OrderOperationalEventConsumer(IObjectMapper objectMapper)
        {
            _objectMapper = objectMapper;
        }

        public async Task Consume(ConsumeContext<SubmitOrderIntegrationEvent> context)
        {
            var orderSubmitedEvent = _objectMapper.Map<SubmitOrderIntegrationEvent, OrderSubmitedEvent>(context.Message);

            await context.Publish(orderSubmitedEvent);
        }

        public async Task Consume(ConsumeContext<FullfillOrderIntegrationEvent> context)
        {
            await context.Publish(new OrderFulfillmentCompletedEvent
            {
                OrderId = context.Message.OrderId,
                ShipmentId = context.Message.ShipmentId,
            });
        }

        public async Task Consume(ConsumeContext<CompleteOrderIntegrationEvent> context)
        {
            await context.Publish(new OrderCompletedEvent
            {
                OrderId = context.Message.OrderId,
                ShippedDate = context.Message.ShippedDate,
            });
        }

        public async Task Consume(ConsumeContext<CancelOrderIntegrationEvent> context)
        {
            await context.Publish(new OrderCancelledEvent
            {
                OrderId = context.Message.OrderId,
                Reason = context.Message.Reason,
                CancellationDate = context.Message.CancellationDate,
            });
        }
    }
}
