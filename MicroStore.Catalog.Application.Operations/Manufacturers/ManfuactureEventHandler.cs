using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace MicroStore.Catalog.Application.Operations.Manufacturers
{
    public class ManfuactureEventHandler : 
        ILocalEventHandler<EntityCreatedEventData<ElasticManufacturerProfile>>,
        ILocalEventHandler<EntityUpdatedArgs<ElasticManufacturerProfile>>,
        ILocalEventHandler<EntityDeletedArgs<ElasticManufacturerProfile>>
    {
        private readonly IBackgroundJobManager _backgroundJopManager;

        public ManfuactureEventHandler(IBackgroundJobManager backgroundJopManager)
        {
            _backgroundJopManager = backgroundJopManager;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<ElasticManufacturerProfile> eventData)
        {
            var args = new EntityCreatedArgs<ElasticManufacturerProfile>(eventData.Entity);
 
            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityUpdatedArgs<ElasticManufacturerProfile> eventData)
        {
            var args = new EntityUpdatedArgs<ElasticManufacturerProfile>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityDeletedArgs<ElasticManufacturerProfile> eventData)
        {
            var args = new EntityDeletedArgs<ElasticManufacturerProfile>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }
    }
}
