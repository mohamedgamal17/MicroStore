using MassTransit;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.Manufacturers
{
    public class ManfuactureEventHandler : 
        ILocalEventHandler<EntityCreatedEventData<Manufacturer>>,
        ILocalEventHandler<EntityUpdatedEventData<Manufacturer>>,
        ILocalEventHandler<EntityDeletedEventData<Manufacturer>>,
        ITransientDependency
    {

        private readonly IObjectMapper _objectMapper;
        private readonly IPublishEndpoint _publishEndPoint;

        public ManfuactureEventHandler(IObjectMapper objectMapper, IPublishEndpoint publishEndPoint)
        {
            _objectMapper = objectMapper;
            _publishEndPoint = publishEndPoint;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<Manufacturer> eventData)
        {
            var eto = _objectMapper.Map<Manufacturer, ManufacturerEto>(eventData.Entity);

            var synchronizationEvent = new EntityCreatedEvent<ManufacturerEto>(eto);
 
            await _publishEndPoint.Publish(synchronizationEvent);
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Manufacturer> eventData)
        {
            var eto = _objectMapper.Map<Manufacturer, ManufacturerEto>(eventData.Entity);

            var synchronizationEvent = new EntityUpdatedEvent<ManufacturerEto>(eto);

            await _publishEndPoint.Publish(synchronizationEvent);
        }

        public async Task HandleEventAsync(EntityDeletedEventData<Manufacturer> eventData)
        {
            var eto = _objectMapper.Map<Manufacturer, ManufacturerEto>(eventData.Entity);

            var synchronizationEvent = new EntityDeletedEvent<ManufacturerEto>(eto);

            await _publishEndPoint.Publish(synchronizationEvent);
        }
    }
}
