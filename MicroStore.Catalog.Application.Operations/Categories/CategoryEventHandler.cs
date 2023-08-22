using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace MicroStore.Catalog.Application.Operations.Categories
{
    public class CategoryEventHandler :
        ILocalEventHandler<EntityCreatedEventData<Category>>,
        ILocalEventHandler<EntityUpdatedEventData<Category>>,
        ILocalEventHandler<EntityDeletedEventData<Category>>

    {
        private readonly IBackgroundJobManager _backgroundJopManager;

        public CategoryEventHandler(IBackgroundJobManager backgroundJopManager)
        {
            _backgroundJopManager = backgroundJopManager;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<Category> eventData)
        {
            var args = new EntityCreatedArgs<Category>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Category> eventData)
        {
            var args = new EntityUpdatedEventData<Category>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityDeletedEventData<Category> eventData)
        {
            var args = new EntityDeletedEventData<Category>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }
    }
}
