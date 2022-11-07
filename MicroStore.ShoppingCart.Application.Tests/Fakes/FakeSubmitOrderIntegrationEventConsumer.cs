using MassTransit;
using MicroStore.Ordering.IntegrationEvents.Responses;
using MicroStore.Ordering.IntegrationEvents;
namespace MicroStore.ShoppingCart.Application.Tests.Fakes
{
    public class FakeSubmitOrderIntegrationEventConsumer : IConsumer<SubmitOrderIntegrationEvent>
    {
        public Task Consume(ConsumeContext<SubmitOrderIntegrationEvent> context)
        {
            return context.RespondAsync(new OrderSubmitedResponse
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber,
                UserId = context.Message.UserId,
                ShippingAddressId = context.Message.ShippingAddressId,
                BillingAddressId = context.Message.BillingAddressId,
                Total = context.Message.TotalPrice,
                SubTotal = context.Message.SubTotal,
                OrderItemModels = context.Message.OrderItems
            });
        }
    }
}
