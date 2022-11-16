using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.Consumers
{
    public class CreatePaymentIntegrationEventConsumer : IConsumer<CreatePaymentIntegrationEvent>
    {

        private ILocalMessageBus _localMessageBus;


        public CreatePaymentIntegrationEventConsumer(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }

   
        public async Task Consume(ConsumeContext<CreatePaymentIntegrationEvent> context)
        {

            CreatePaymentRequestCommand createPaymentCommand = new CreatePaymentRequestCommand
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber,
                Amount = context.Message.TotalPrice,
                CustomerId = context.Message.CustomerId
            };

            var result = await _localMessageBus.Send(createPaymentCommand);

            PaymentCreatedIntegrationEvent paymentCreatedIntegrationEvent = new PaymentCreatedIntegrationEvent
            {
                PaymentId = result.PaymentId.ToString(),
                CustomerId = result.CustomerId,
                OrderId = result.OrderId,
                OrderNubmer = result.OrderNumber,         
            };

            await context.Publish(paymentCreatedIntegrationEvent);
        }

    }
}
