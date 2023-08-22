using Elastic.Clients.Elasticsearch;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Operations.Products
{
    public class ElasticProductSynchronizationJob :
        IAsyncBackgroundJob<EntityCreatedArgs<Product>>,
        IAsyncBackgroundJob<EntityUpdatedArgs<Product>>
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;

        public ElasticProductSynchronizationJob(ElasticsearchClient elasticsearchClient, IObjectMapper objectMapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _objectMapper = objectMapper;
        }

        public async Task ExecuteAsync(EntityCreatedArgs<Product> args)
        {
            var elasticEntity = _objectMapper.Map<Product, ElasticProduct>(args.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task ExecuteAsync(EntityUpdatedArgs<Product> args)
        {
            var elasticEntity = _objectMapper.Map<Product, ElasticProduct>(args.Entity);

            var response = await _elasticsearchClient.UpdateAsync<ElasticProduct, object>(ElasticEntitiesConsts.CategoryIndex, elasticEntity.Id, des =>
                 des.Doc(elasticEntity)
             );
        }
    }
}
