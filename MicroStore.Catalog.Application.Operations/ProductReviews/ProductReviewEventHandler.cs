using Elastic.Clients.Elasticsearch;
using MicroStore.Catalog.Application.Operations.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
namespace MicroStore.Catalog.Application.Operations.ProductReviews
{
    public class ProductReviewEventHandler :
        ILocalEventHandler<EntityCreatedEventData<ProductReview>>,
        ILocalEventHandler<EntityUpdatedEventData<ProductReview>>,
        ILocalEventHandler<EntityDeletedEventData<ProductReview>>,
        ITransientDependency
    {

        private readonly ElasticsearchClient _elasticsearchClient;

        public ProductReviewEventHandler(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<ProductReview> eventData)
        {
            var elasticEntity = PrepareElasticProductReview(eventData.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);

            response.ThrowIfFailure();
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<ProductReview> eventData)
        {
            var elasticEntity = PrepareElasticProductReview(eventData.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);

            response.ThrowIfFailure();
        }

        public async Task HandleEventAsync(EntityDeletedEventData<ProductReview> eventData)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticProductReview>(eventData.Entity.Id);

            response.ThrowIfFailure();
        }

        private ElasticProductReview PrepareElasticProductReview(ProductReview productReview)
        {
            var elasticEntity = new ElasticProductReview
            {
                Id = productReview.Id,
                Title = productReview.Title,
                ProductId = productReview.ProductId,
                Rating = productReview.Rating,
                ReplayText = productReview.ReplayText,
                ReviewText = productReview.ReviewText,
                UserId = productReview.UserId,
                CreationTime = productReview.CreationTime,
                CreatorId = productReview.CreatorId?.ToString(),
                LastModificationTime = productReview.LastModificationTime,
                LastModifierId = productReview.LastModifierId.ToString(),
                DeleterId = productReview.DeleterId?.ToString(),
                DeletionTime = productReview.DeletionTime,
                IsDeleted = productReview.IsDeleted
            };

            return elasticEntity;
        }
    }
}
