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
                ShippingAddress = context.Message.ShippingAddress.MapAddress(),
                BillingAddress = context.Message.BillingAddress.MapAddress(),
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
                ExternalProductId = x.ExternalProductId,
                Sku  = x.Sku,
                Name = x.Name,
                Thumbnail = x.Thumbnail,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice

            }).ToList();
        }


        public static MicroStore.Ordering.Events.Models.AddressModel MapAddress(this MicroStore.Ordering.IntegrationEvents.Models.AddressModel address)
        {
            return new MicroStore.Ordering.Events.Models.AddressModel
            {
                CountryCode = address.CountryCode,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                Zip = address.Zip,
                Name = address.Name,
                Phone = address.Phone
            };
        }
        public static List<OrderItemResponseModel> MapOrderItemResponse( this List<MicroStore.Ordering.IntegrationEvents.Models.OrderItemModel> items)
        {
            return items.Select(x => new OrderItemResponseModel
            {
                ExternalProductId = x.ExternalProductId,
                Name = x.Name,
                Thumbnail = x.Thumbnail,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
                
            }).ToList();
        }
    }
}
