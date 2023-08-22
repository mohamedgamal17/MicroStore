using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
namespace MicroStore.Catalog.Application.Operations.ProductTags
{
    public class SpecificationAttributeEventHandler :
        ILocalEventHandler<EntityCreatedEventData<SpecificationAttribute>>,
        ILocalEventHandler<EntityUpdatedEventData<SpecificationAttribute>>,
        ILocalEventHandler<EntityDeletedEventData<SpecificationAttribute>>

    {
        private readonly IBackgroundJobManager _backgroundJopManager;

        public SpecificationAttributeEventHandler(IBackgroundJobManager backgroundJopManager)
        {
            _backgroundJopManager = backgroundJopManager;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<SpecificationAttribute> eventData)
        {
            var args = new EntityCreatedArgs<SpecificationAttribute>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<SpecificationAttribute> eventData)
        {
            var args = new EntityUpdatedEventData<SpecificationAttribute>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityDeletedEventData<SpecificationAttribute> eventData)
        {
            var args = new EntityDeletedEventData<SpecificationAttribute>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }
    }
}
