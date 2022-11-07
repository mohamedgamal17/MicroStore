using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;
namespace MicroStore.Ordering.Application.Consumers
{
    public class SubmitOrderIntegrationEventConsumer : IConsumer<SubmitOrderIntegrationEvent>
    {
        public Task Consume(ConsumeContext<SubmitOrderIntegrationEvent> context)
        {
            return context.Publish(new OrderSubmitedEvent
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber,
                ShippingAddressId = context.Message.ShippingAddressId,
                BillingAddressId = context.Message.BillingAddressId,
                UserId = context.Message.UserId,
                SubTotal = context.Message.SubTotal,
                Total = context.Message.TotalPrice,
                SubmissionDate = context.Message.SubmissionDate,
                OrderItems = context.Message.OrderItems.MapOrderItem()
            });
        }
    }


    internal static class OrderItemModelExtesnions
    {
        public static List<MicroStore.Ordering.Events.Models.OrderItemModel> MapOrderItem(this List<MicroStore.Ordering.IntegrationEvents.Models.OrderItemModel> items)
        {
            return items.Select(x => new MicroStore.Ordering.Events.Models.OrderItemModel
            {
                ProductId = x.ProductId,
                ItemName = x.ItemName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice

            }).ToList();
        }
    }
}
