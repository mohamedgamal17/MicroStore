using MassTransit;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Operations.ProductTags
{
    public class ProductTagEventHandler :
        ILocalEventHandler<EntityCreatedEventData<ProductTag>>,
        ILocalEventHandler<EntityUpdatedEventData<ProductTag>>,
        ILocalEventHandler<EntityDeletedEventData<ProductTag>>,
        ITransientDependency

    {

        private readonly IObjectMapper _objectMapper;
        private readonly IPublishEndpoint _publishEndPoint;

        public ProductTagEventHandler(IObjectMapper objectMapper, IPublishEndpoint publishEndPoint)
        {
            _objectMapper = objectMapper;
            _publishEndPoint = publishEndPoint;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<ProductTag> eventData)
        {
            var eto = _objectMapper.Map<ProductTag, ProductTagEto>(eventData.Entity);

            var distributedEvent = new EntityCreatedEvent<ProductTagEto>(eto);

            await _publishEndPoint.Publish(distributedEvent);
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<ProductTag> eventData)
        {
            var eto = _objectMapper.Map<ProductTag, ProductTagEto>(eventData.Entity);

            var distributedEvent = new EntityUpdatedEvent<ProductTagEto>(eto);

            await _publishEndPoint.Publish(distributedEvent);
        }

        public async Task HandleEventAsync(EntityDeletedEventData<ProductTag> eventData)
        {
            var eto = _objectMapper.Map<ProductTag, ProductTagEto>(eventData.Entity);

            var distributedEvent = new EntityDeletedEvent<ProductTagEto>(eto);

            await _publishEndPoint.Publish(distributedEvent);
        }
    }
}
