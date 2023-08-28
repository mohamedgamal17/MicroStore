using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.ProductReviews
{
    public class ElasticProductReviewSyncronizationHandler :
        IConsumer<EntityCreatedEvent<ProductReviewEto>>,
        IConsumer<EntityUpdatedEvent<ProductReviewEto>>,
        IConsumer<EntityDeletedEvent<ProductReviewEto>>,
        ITransientDependency
    {

        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;

        public ElasticProductReviewSyncronizationHandler(ElasticsearchClient elasticsearchClient, IObjectMapper objectMapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _objectMapper = objectMapper;
        }

        public async Task Consume(ConsumeContext<EntityCreatedEvent<ProductReviewEto>> context)
        {
            var elasticEntity = _objectMapper.Map<ProductReviewEto, ElasticProductReview>(context.Message.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task Consume(ConsumeContext<EntityUpdatedEvent<ProductReviewEto>> context)
        {
            var elasticEntity = _objectMapper.Map<ProductReviewEto, ElasticProductReview>(context.Message.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task Consume(ConsumeContext<EntityDeletedEvent<ProductReviewEto>> context)
        {
            var response = await _elasticsearchClient.DeleteAsync<ProductReview>(context.Message.Entity.Id);
        }

    }
}
