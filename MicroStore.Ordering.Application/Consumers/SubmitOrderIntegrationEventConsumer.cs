using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.IntegrationEvents;
namespace MicroStore.Ordering.Application.Consumers
{
    public class SubmitOrderIntegrationEventConsumer : IConsumer<SubmitOrderIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<SubmitOrderIntegrationEvent> context)
        {
            await context.Publish(new OrderSubmitedEvent
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber,
                ShippingAddressId = context.Message.ShippingAddressId,
                BillingAddressId = context.Message.BillingAddressId,
                UserId = context.Message.UserId,
                ShippingCost = context.Message.ShippingCost,
                TaxCost = context.Message.TaxCost,
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
                ProductImage = x.ProductImage,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice

            }).ToList();
        }



        public static List<OrderItemResponseModel> MapOrderItemResponse( this List<MicroStore.Ordering.IntegrationEvents.Models.OrderItemModel> items)
        {
            return items.Select(x => new OrderItemResponseModel
            {
                ProductId = x.ProductId,
                ItemName = x.ItemName,
                ProductImage = x.ProductImage,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
                
            }).ToList();
        }
    }
}
