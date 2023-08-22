using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Catalog.IntegrationEvents.Models;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
namespace MicroStore.Catalog.Application.Operations.Products
{
    public class ProductEventHandler :
        ILocalEventHandler<EntityCreatedEventData<Product>>,
        ILocalEventHandler<EntityUpdatedEventData<Product>>
    {
        private readonly IPublishEndpoint _publishEndPoint;

        private readonly IBackgroundJobManager _backgroundJobManager;
        public ProductEventHandler(IPublishEndpoint publishEndPoint, IBackgroundJobManager backgroundJobManager)
        {
            _publishEndPoint = publishEndPoint;
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<Product> eventData)
        {
            var args = new EntityCreatedArgs<Product>(eventData.Entity);

            var integrationEvent = PrepareProductCreatedIntegrationEvent(eventData.Entity);

            await _backgroundJobManager.EnqueueAsync(args);

            await _publishEndPoint.Publish(integrationEvent);
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Product> eventData)
        {
            var args = new EntityUpdatedArgs<Product>(eventData.Entity);

            var integrationEvent = PreapreProductUpdateIntegrationEvent(eventData.Entity);

            await _backgroundJobManager.EnqueueAsync(args);

            await _publishEndPoint.Publish(integrationEvent);
        }

        private ProductCreatedIntegrationEvent PrepareProductCreatedIntegrationEvent(Product product)
        {
            var integrationEvent = new ProductCreatedIntegrationEvent
            {
                ProductId = product.Id.ToString(),
                Sku = product.Sku.ToString(),
                Name = product.Name,
                Description = product.ShortDescription,
                Price = product.Price,

            };

            if (product.ProductImages != null)
            {
                integrationEvent.ProductImages = product.ProductImages.Select(x => new ProductImageModel
                {
                    ImageLink = x.ImagePath,
                    DisplayOrder = x.DisplayOrder
                }).ToList();
            }

            return integrationEvent;
        }


        private ProductUpdatedIntegerationEvent PreapreProductUpdateIntegrationEvent(Product product)
        {
            var integrationEvent = new ProductUpdatedIntegerationEvent
            {
                ProductId = product.Id.ToString(),
                Sku = product.Sku.ToString(),
                Name = product.Name,
                Description = product.ShortDescription,
                Price = product.Price,

            };

            if (product.ProductImages != null)
            {
                integrationEvent.ProductImages = product.ProductImages.Select(x => new ProductImageModel
                {
                    ImageLink = x.ImagePath,
                    DisplayOrder = x.DisplayOrder
                }).ToList();
            }


            return integrationEvent;
        }
    }
}
