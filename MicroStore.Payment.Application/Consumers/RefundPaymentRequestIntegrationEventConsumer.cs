﻿using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.IntegrationEvents;


namespace MicroStore.Payment.Application.Consumers
{
    [Obsolete]
    public class RefundPaymentRequestIntegrationEventConsumer : IConsumer<RefundPaymentIntegrationEvent>
    {

        private readonly ILocalMessageBus _localMessageBus;

        public RefundPaymentRequestIntegrationEventConsumer(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }

        public Task Consume(ConsumeContext<RefundPaymentIntegrationEvent> context)
        {
            RefundPaymentRequestCommand voidPaymentCommand = new RefundPaymentRequestCommand
            {
                PaymentId = Guid.Parse(context.Message.PaymentId),
            };

            return _localMessageBus.Send(voidPaymentCommand);
        }
    }
}