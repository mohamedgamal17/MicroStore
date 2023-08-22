using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace MicroStore.Catalog.Application.Operations.ProductTags
{
    public class ProductTagEventHandler :
        ILocalEventHandler<EntityCreatedEventData<ProductTag>>,
        ILocalEventHandler<EntityUpdatedEventData<ProductTag>>,
        ILocalEventHandler<EntityDeletedEventData<ProductTag>>

    {
        private readonly IBackgroundJobManager _backgroundJopManager;

        public ProductTagEventHandler(IBackgroundJobManager backgroundJopManager)
        {
            _backgroundJopManager = backgroundJopManager;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<ProductTag> eventData)
        {
            var args = new EntityCreatedArgs<ProductTag>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<ProductTag> eventData)
        {
            var args = new EntityUpdatedEventData<ProductTag>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityDeletedEventData<ProductTag> eventData)
        {
            var args = new EntityDeletedEventData<ProductTag>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }
    }
}
