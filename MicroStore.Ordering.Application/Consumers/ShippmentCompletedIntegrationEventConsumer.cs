//using MassTransit;
//using MicroStore.Ordering.Events;
//namespace MicroStore.Ordering.Application.Consumers
//{
//    public class ShippmentCompletedIntegrationEventConsumer : IConsumer<ShippmentCompletedIntegrationEvent>
//    {
//        public Task Consume(ConsumeContext<ShippmentCompletedIntegrationEvent> context)
//        {
//            return context.Publish(new OrderCompletedEvent
//            {
//                OrderId = context.Message.OrderId,
//                OrderNumber = context.Message.OrderNumber,
//                ShippmentId = context.Message.ShippmentId,

//            });
//        }
//    }
//}
