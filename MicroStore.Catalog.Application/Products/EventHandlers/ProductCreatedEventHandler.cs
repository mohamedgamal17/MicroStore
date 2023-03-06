using MassTransit;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Catalog.IntegrationEvents.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Threading;

namespace MicroStore.Catalog.Application.Products.EventHandlers
{
    public class ProductCreatedEventHandler : ILocalEventHandler<EntityCreatedEventData<Product>> ,ITransientDependency
    {
        private readonly IPublishEndpoint _publishEndPoint;

        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        public ProductCreatedEventHandler(IPublishEndpoint publishEndPoint, ICancellationTokenProvider cancellationTokenProvider)
        {
            _publishEndPoint = publishEndPoint;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public Task HandleEventAsync(EntityCreatedEventData<Product> eventData)
        {
            var integrationEvent = new ProductCreatedIntegrationEvent
            {
                ProductId = eventData.Entity.Id.ToString(),
                Sku = eventData.Entity.Sku.ToString(),
                Name = eventData.Entity.Name,
                Description = eventData.Entity.ShortDescription,
                Price = eventData.Entity.Price,
                
            };

            if(eventData.Entity.ProductImages != null)
            {
                integrationEvent.ProductImages = eventData.Entity.ProductImages.Select(x => new ProductImageModel
                {
                    ImageLink = x.ImagePath,
                    DisplayOrder = x.DisplayOrder
                }).ToList();
            }

            return _publishEndPoint.Publish(integrationEvent,_cancellationTokenProvider.Token);
        }
    }
}
