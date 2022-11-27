//using MassTransit;
//using MicroStore.Ordering.Events;
//using MicroStore.Shipping.IntegrationEvent;
//namespace MicroStore.Ordering.Application.Consumers
//{
//    public class ShippmentFaildIntegrationEventConsumer : IConsumer<ShippmentFaildIntegrationEvent>
//    {
//        public Task Consume(ConsumeContext<ShippmentFaildIntegrationEvent> context)
//        {
//            return context.Publish(new OrderShippmentFailedEvent
//            {
//                OrderId = context.Message.OrderId,
//                OrderNumber = context.Message.OrderNumber,
//                ShippmentId = context.Message.ShippmentId,
//                FaultDate = context.Message.FaultDate,
//                Reason = context.Message.FaultReason
//            });
//        }
//    }
//}
