using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Domain.Events;
using MicroStore.Catalog.IntegrationEvents;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace MicroStore.Catalog.Application.Products.EventHandlers
{
    internal class CreateProductEventHandler : ILocalEventHandler<CreateProductEvent>, ITransientDependency
    {

        private readonly ILogger<CreateProductEventHandler> _logger;

        private readonly IPublishEndpoint _publisherEndPoint;



        public CreateProductEventHandler(ILogger<CreateProductEventHandler> logger, IPublishEndpoint publisherEndPoint)
        {
            _logger = logger;
            _publisherEndPoint = publisherEndPoint;
        }



        public async Task HandleEventAsync(CreateProductEvent eventData)
        {
            _logger.LogInformation("MicroStore Publishing Integration Event: {IntegrationEvent} {@Event}",
               eventData.GetType().Name, eventData);

            var integratedEvent = new CreateProductIntegrationEvent(eventData.ProductId, eventData.Name, eventData.Sku, eventData.Price);

            await _publisherEndPoint.Publish(integratedEvent);
        }
    }
}
