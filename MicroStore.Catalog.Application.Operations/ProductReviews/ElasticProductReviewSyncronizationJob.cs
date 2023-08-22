using Elastic.Clients.Elasticsearch;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.Manufacturers
{
    public class ElasticProductReviewSyncronizationJob :
        IAsyncBackgroundJob<EntityCreatedArgs<ProductReview>>,
        IAsyncBackgroundJob<EntityUpdatedArgs<ProductReview>>,
        IAsyncBackgroundJob<EntityDeletedArgs<ProductReview>>

    {

        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;

        public ElasticProductReviewSyncronizationJob(ElasticsearchClient elasticsearchClient, IObjectMapper objectMapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _objectMapper = objectMapper;
        }

        public async Task ExecuteAsync(EntityCreatedArgs<ProductReview> args)
        {
            var elasticEntity = _objectMapper.Map<ProductReview, ElasticProductReview>(args.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task ExecuteAsync(EntityUpdatedArgs<ProductReview> args)
        {
            var elasticEntity = _objectMapper.Map<ProductReview, ElasticProductReview>(args.Entity);

            var response = await _elasticsearchClient.UpdateAsync<ElasticProductReview, object>(ElasticEntitiesConsts.ProductReviewIndex, elasticEntity.Id, des =>
                 des.Doc(elasticEntity)
             );
        }

        public async Task ExecuteAsync(EntityDeletedArgs<ProductReview> args)
        {
            var response = await _elasticsearchClient.DeleteAsync<ProductReview>(args.Entity.Id);
        }
    }
}
