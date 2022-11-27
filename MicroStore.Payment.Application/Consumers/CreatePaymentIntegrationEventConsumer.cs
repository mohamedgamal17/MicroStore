using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.IntegrationEvents;
using MicroStore.Payment.IntegrationEvents.Models;
namespace MicroStore.Payment.Application.Consumers
{
    public class CreatePaymentIntegrationEventConsumer : IConsumer<CreatePaymentRequestIntegrationEvent>
    {

        private ILocalMessageBus _localMessageBus;


        public CreatePaymentIntegrationEventConsumer(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }

   
        public async Task Consume(ConsumeContext<CreatePaymentRequestIntegrationEvent> context)
        {
            CreatePaymentRequestCommand createPaymentCommand = new CreatePaymentRequestCommand
            {
                OrderId = context.Message.OrderId,
                OrderNubmer = context.Message.OrderNumber,
                ShippingCost = context.Message.ShippingCost,
                TaxCost = context.Message.TaxCost,
                SubtTotal= context.Message.SubTotal,
                TotalCost = context.Message.TotalCost,
                UserId = context.Message.UserId,
                Items = MapOrderItems(context.Message.Items)
            };


            var result = await _localMessageBus.Send(createPaymentCommand,context.CancellationToken);

            PaymentCreatedIntegrationEvent paymentCreatedIntegrationEvent = new PaymentCreatedIntegrationEvent
            {
                PaymentId = result.PaymentId.ToString(),
                CustomerId = result.CustomerId,
                OrderId = result.OrderId,
                OrderNubmer = result.OrderNumber,
            };

            await context.Publish(paymentCreatedIntegrationEvent,context.CancellationToken);
        }


        private List<OrderItemModel> MapOrderItems(List<PaymentRequestProductModel> items)
        {
            return items.Select(x => new OrderItemModel
            {

                ProductId = x.ProductId,
                Sku = x.Sku,
                Name = x.Name,
                Image = x.Image,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            }).ToList();
        }
    }
}
