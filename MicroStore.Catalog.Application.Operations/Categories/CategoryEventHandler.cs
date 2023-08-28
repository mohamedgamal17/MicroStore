using MassTransit;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Operations.Categories
{
    public class CategoryEventHandler :
        ILocalEventHandler<EntityCreatedEventData<Category>>,
        ILocalEventHandler<EntityUpdatedEventData<Category>>,
        ILocalEventHandler<EntityDeletedEventData<Category>>,
        ITransientDependency

    {

        private readonly IObjectMapper _objectMapper;

        private readonly IPublishEndpoint _PublishEndPoint;
        public CategoryEventHandler(IObjectMapper objectMapper, IPublishEndpoint publishEndPoint)
        {
            _objectMapper = objectMapper;
            _PublishEndPoint = publishEndPoint;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<Category> eventData)
        {
            var eto = _objectMapper.Map<Category, CategoryEto>(eventData.Entity);

            var synchronizationEvent = new EntityCreatedEvent<CategoryEto>(eto);

            await _PublishEndPoint.Publish(synchronizationEvent);

        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Category> eventData)
        {
            var eto = _objectMapper.Map<Category, CategoryEto>(eventData.Entity);

            var synchronizationEvent = new EntityUpdatedEvent<CategoryEto>(eto);

            await _PublishEndPoint.Publish(synchronizationEvent);

        }

        public async Task HandleEventAsync(EntityDeletedEventData<Category> eventData)
        {
            var eto = _objectMapper.Map<Category, CategoryEto>(eventData.Entity);

            var synchronizationEvent = new EntityDeletedEvent<CategoryEto>(eto);

            await _PublishEndPoint.Publish(synchronizationEvent);
        }


      
    }
}
