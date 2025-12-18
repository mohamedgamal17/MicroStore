using MassTransit;
using MicroStore.Payment.Domain.Shared.Events;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace MicroStore.Payment.Application.EventHandlers
{
    public class PaymentAcceptedEventHandler : ILocalEventHandler<PaymentAcceptedEvent>, ITransientDependency
    {

        private readonly IPublishEndpoint _publishEndPoint;

        public PaymentAcceptedEventHandler(IPublishEndpoint publishEndPoint)
        {
            _publishEndPoint = publishEndPoint;
        }

        public Task HandleEventAsync(PaymentAcceptedEvent eventData)
        {
            return _publishEndPoint.Publish(new PaymentAccepetedIntegrationEvent
            {
                OrderId = eventData.OrderId,
                UserId = eventData.UserName,
                PaymentId = eventData.PaymentId.ToString(),
                TransactionId = eventData.TransactionId,
                PaymentGateway = eventData.PaymentGateway
            });
        }
    }
}
