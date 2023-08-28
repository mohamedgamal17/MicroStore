using MassTransit;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Operations.SpecificationAttributes
{
    public class SpecificationAttributeEventHandler :
        ILocalEventHandler<EntityCreatedEventData<SpecificationAttribute>>,
        ILocalEventHandler<EntityUpdatedEventData<SpecificationAttribute>>,
        ILocalEventHandler<EntityDeletedEventData<SpecificationAttribute>>,
        ITransientDependency

    {
        private readonly IObjectMapper _objectMapper;

        private readonly IPublishEndpoint _publishEndPoint;

        public SpecificationAttributeEventHandler(IObjectMapper objectMapper, IPublishEndpoint publishEndPoint)
        {
            _objectMapper = objectMapper;
            _publishEndPoint = publishEndPoint;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<SpecificationAttribute> eventData)
        {
            var eto = _objectMapper.Map<SpecificationAttribute, SpecificationAttributeEto>(eventData.Entity);

            var synchroinzationEvent = new EntityCreatedEvent<SpecificationAttributeEto>(eto);

            await _publishEndPoint.Publish(synchroinzationEvent);
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<SpecificationAttribute> eventData)
        {
            var eto = _objectMapper.Map<SpecificationAttribute, SpecificationAttributeEto>(eventData.Entity);

            var synchroinzationEvent = new EntityUpdatedEvent<SpecificationAttributeEto>(eto);

            await _publishEndPoint.Publish(synchroinzationEvent);
        }

        public async Task HandleEventAsync(EntityDeletedEventData<SpecificationAttribute> eventData)
        {
            var eto = _objectMapper.Map<SpecificationAttribute, SpecificationAttributeEto>(eventData.Entity);

            var synchroinzationEvent = new EntityDeletedEvent<SpecificationAttributeEto>(eto);

            await _publishEndPoint.Publish(synchroinzationEvent);
        }
    }
}
