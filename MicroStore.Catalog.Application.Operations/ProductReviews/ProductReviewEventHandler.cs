using MassTransit;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Operations.ProductReviews
{
    public class ProductReviewEventHandler :
        ILocalEventHandler<EntityCreatedEventData<ProductReview>>,
        ILocalEventHandler<EntityUpdatedEventData<ProductReview>>,
        ILocalEventHandler<EntityDeletedEventData<ProductReview>>,
        ITransientDependency
    {
        private readonly IObjectMapper _objectMapper;

        private readonly IPublishEndpoint _publishEndPoint;

        public ProductReviewEventHandler(IObjectMapper objectMapper, IPublishEndpoint publishEndPoint)
        {
            _objectMapper = objectMapper;
            _publishEndPoint = publishEndPoint;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<ProductReview> eventData)
        {
            var eto = _objectMapper.Map<ProductReview, ProductReviewEto>(eventData.Entity);

            var synchronizationEvent = new EntityCreatedEvent<ProductReviewEto>(eto);

            await _publishEndPoint.Publish(synchronizationEvent);
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<ProductReview> eventData)
        {
            var eto = _objectMapper.Map<ProductReview, ProductReviewEto>(eventData.Entity);

            var synchronizationEvent = new EntityUpdatedEvent<ProductReviewEto>(eto);

            await _publishEndPoint.Publish(synchronizationEvent);
        }

        public async Task HandleEventAsync(EntityDeletedEventData<ProductReview> eventData)
        {
            var eto = _objectMapper.Map<ProductReview, ProductReviewEto>(eventData.Entity);

            var synchronizationEvent = new EntityDeletedEvent<ProductReviewEto>(eto);

            await _publishEndPoint.Publish(synchronizationEvent);
        }
    }
}
