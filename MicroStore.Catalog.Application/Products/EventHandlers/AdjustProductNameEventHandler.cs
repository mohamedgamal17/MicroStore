using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Domain.Events;
using MicroStore.Catalog.IntegrationEvents;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace MicroStore.Catalog.Application.Products.EventHandlers
{
    public class AdjustProductNameEventHandler : ILocalEventHandler<AdjustProductNameEvent>, ITransientDependency
    {
        private readonly ILogger<AdjustProductNameEventHandler> _logger;

        private readonly IPublishEndpoint _publisherEndPoint;

        public AdjustProductNameEventHandler(ILogger<AdjustProductNameEventHandler> logger, IPublishEndpoint publisherEndPoint)
        {
            _logger = logger;
            _publisherEndPoint = publisherEndPoint;
        }

        public async Task HandleEventAsync(AdjustProductNameEvent eventData)
        {
            _logger.LogDebug("MicroStore Publishing Integration Event: {IntegrationEvent} {@Event}",
               eventData.GetType().Name, eventData);

            var integratedEvent = new AdjustProductNameIntegrationEvent(eventData.ProductId, eventData.Name);

            await _publisherEndPoint.Publish(integratedEvent);
        }
    }
}
