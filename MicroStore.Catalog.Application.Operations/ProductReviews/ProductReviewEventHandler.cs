using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace MicroStore.Catalog.Application.Operations.Manufacturers
{
    public class ProductReviewEventHandler : 
        ILocalEventHandler<EntityCreatedEventData<ProductReview>>,
        ILocalEventHandler<EntityUpdatedArgs<ProductReview>>,
        ILocalEventHandler<EntityDeletedArgs<ProductReview>>
    {
        private readonly IBackgroundJobManager _backgroundJopManager;
        public ProductReviewEventHandler(IBackgroundJobManager backgroundJopManager)
        {
            _backgroundJopManager = backgroundJopManager;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<ProductReview> eventData)
        {
            var args = new EntityCreatedArgs<ProductReview>(eventData.Entity);
 
            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityUpdatedArgs<ProductReview> eventData)
        {
            var args = new EntityUpdatedArgs<ProductReview>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }

        public async Task HandleEventAsync(EntityDeletedArgs<ProductReview> eventData)
        {
            var args = new EntityDeletedArgs<ProductReview>(eventData.Entity);

            await _backgroundJopManager.EnqueueAsync(args);
        }
    }
}
