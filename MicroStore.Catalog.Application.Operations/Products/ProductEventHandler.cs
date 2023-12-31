using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Catalog.IntegrationEvents.Models;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Operations.Products
{
    public class ProductEventHandler :
        ILocalEventHandler<EntityCreatedEventData<Product>>,
        ILocalEventHandler<EntityUpdatedEventData<Product>>,
        ITransientDependency
    {
        private readonly IObjectMapper _objectMapper;

        private readonly IPublishEndpoint _publishEndPoint;

        private readonly IRepository<Product> _productsRepository;
        public ProductEventHandler(IObjectMapper objectMapper, IPublishEndpoint publishEndPoint, IRepository<Product> productsRepository)
        {
            _objectMapper = objectMapper;
            _publishEndPoint = publishEndPoint;
            _productsRepository = productsRepository;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<Product> eventData)
        {
            var product = await _productsRepository.SingleAsync(x=> x.Id == eventData.Entity.Id);

            var eto = _objectMapper.Map<Product, ProductEto>(product);

          
            var synchronizationEvent =  new EntityCreatedEvent<ProductEto>(eto);

            var integrationEvent = PrepareProductCreatedIntegrationEvent(eventData.Entity);

            await _publishEndPoint.Publish(synchronizationEvent);

            await _publishEndPoint.Publish(integrationEvent);
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Product> eventData)
        {
            var product = await _productsRepository.SingleAsync(x => x.Id == eventData.Entity.Id);

            var eto = _objectMapper.Map<Product, ProductEto>(product);

            var synchronizationEvent = new EntityUpdatedEvent<ProductEto>(eto);

            var integrationEvent = PreapreProductUpdateIntegrationEvent(eventData.Entity);

            await _publishEndPoint.Publish(synchronizationEvent);

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
                    ImageLink = x.Image,
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
                UnitPrice = product.Price,

            };

            if (product.ProductImages != null)
            {
                integrationEvent.ProductImages = product.ProductImages.Select(x => new ProductImageModel
                {
                    ImageLink = x.Image,
                    DisplayOrder = x.DisplayOrder
                }).ToList();
            }


            return integrationEvent;
        }
    }
}
