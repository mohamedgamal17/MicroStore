using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Domain.Events;
using MicroStore.Catalog.IntegrationEvents;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace MicroStore.Catalog.Application.Products.EventHandlers
{
    public class AdjustProductPriceEventHandler : ILocalEventHandler<AdjustProductPriceEvent>, ITransientDependency
    {

        private readonly IPublishEndpoint _publisherEndpoint;

        private readonly ILogger<AdjustProductPriceEventHandler> _logger;

        public AdjustProductPriceEventHandler(IPublishEndpoint publisherEndpoint, ILogger<AdjustProductPriceEventHandler> logger)
        {
            _publisherEndpoint = publisherEndpoint;
            _logger = logger;
        }

        public async Task HandleEventAsync(AdjustProductPriceEvent eventData)
        {
            _logger.LogDebug("MicroStore Publishing Integration Event: {IntegrationEvent} {@Event}",
                 eventData.GetType().Name, eventData);


            var integrationEvent = new AdjustProductPriceIntegrationEvent(eventData.ProductId, eventData.Price);

            await _publisherEndpoint.Publish(integrationEvent);
        }
    }
}
