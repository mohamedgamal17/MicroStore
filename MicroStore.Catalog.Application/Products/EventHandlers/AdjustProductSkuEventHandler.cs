using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Domain.Events;
using MicroStore.Catalog.IntegrationEvents;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace MicroStore.Catalog.Application.Products.EventHandlers
{
    public class AdjustProductSkuEventHandler : ILocalEventHandler<AdjustProductSkuEvent>, ITransientDependency
    {
        private readonly ILogger<AdjustProductSkuEventHandler> _logger;

        private readonly IPublishEndpoint _publishEndpoint;

        public AdjustProductSkuEventHandler(ILogger<AdjustProductSkuEventHandler> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }



        public async Task HandleEventAsync(AdjustProductSkuEvent eventData)
        {
            _logger.LogInformation("MicroStore Publishing Integration Event: {IntegrationEvent} {@Event}",
               eventData.GetType().Name, eventData);

            var integratedEvent = new AdjustProductSkuIntegrationEvent(eventData.ProductId, eventData.Sku);

            await _publishEndpoint.Publish(integratedEvent);
        }
    }
}
