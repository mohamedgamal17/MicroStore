using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.Categories
{
    public class ElasticProductTagSyncronizationJob :
        IAsyncBackgroundJob<EntityCreatedArgs<ProductTag>>,
        IAsyncBackgroundJob<EntityUpdatedArgs<ProductTag>>,
        IAsyncBackgroundJob<EntityDeletedArgs<ProductTag>>
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;
        private readonly ILogger<ElasticCategorySyncronizationJob> _logger;

        public ElasticProductTagSyncronizationJob(ElasticsearchClient elasticsearchClient, ILogger<ElasticCategorySyncronizationJob> logger, IObjectMapper mapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _logger = logger;
            _objectMapper = mapper;
        }

        public async Task ExecuteAsync(EntityCreatedArgs<ProductTag> args)
        {
            var elasticEntity = _objectMapper.Map<ProductTag, ElasticProductTag>(args.Entity);

            var response =  await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task ExecuteAsync(EntityUpdatedArgs<ProductTag> args)
        {
            var elasticEntity = _objectMapper.Map<ProductTag, ElasticProductTag>(args.Entity);

           var response = await _elasticsearchClient.UpdateAsync<ElasticProductTag, object>(ElasticEntitiesConsts.ProductTagIndex, elasticEntity.Id, des =>
                des.Doc(elasticEntity)
            );
        }

        public async Task ExecuteAsync(EntityDeletedArgs<ProductTag> args)
        {
            var response = await _elasticsearchClient.DeleteAsync<ProductTag>(args.Entity.Id);
        }
    }
}
