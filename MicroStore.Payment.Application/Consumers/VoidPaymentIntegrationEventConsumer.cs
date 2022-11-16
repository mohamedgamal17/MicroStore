using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.Consumers
{
    public class VoidPaymentIntegrationEventConsumer : IConsumer<VoidPaymentIntegrationEvent>
    {

        private readonly ILocalMessageBus _localMessageBus;

        public VoidPaymentIntegrationEventConsumer(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }

        public Task Consume(ConsumeContext<VoidPaymentIntegrationEvent> context)
        {
            VoidPaymentCommand voidPaymentCommand = new VoidPaymentCommand
            {
                PaymentId = Guid.Parse(context.Message.PaymentId),
                FaultDate = context.Message.FaultDate
            };

            return _localMessageBus.Send(voidPaymentCommand);
        }
    }
}
