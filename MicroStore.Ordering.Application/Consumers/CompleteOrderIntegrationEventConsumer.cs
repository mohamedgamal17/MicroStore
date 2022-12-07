using MassTransit;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Abstractions.Consts;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.IntegrationEvents;
namespace MicroStore.Ordering.Application.Consumers
{
    public class CompleteOrderIntegrationEventConsumer : IConsumer<CompleteOrderIntegrationEvent>
    {

        public  Task Consume(ConsumeContext<CompleteOrderIntegrationEvent> context)
        {

            return context.Publish(new OrderCompletedEvent
            {
                OrderId = context.Message.OrderId,
                ShippedDate = context.Message.ShippedDate,
            });

        }
    }
}
