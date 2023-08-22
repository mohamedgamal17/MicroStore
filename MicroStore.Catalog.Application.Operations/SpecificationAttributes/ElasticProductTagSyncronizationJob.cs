using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.Categories
{
    public class ElasticSpecificationAttributeSyncronizationJob :
        IAsyncBackgroundJob<EntityCreatedArgs<SpecificationAttribute>>,
        IAsyncBackgroundJob<EntityUpdatedArgs<SpecificationAttribute>>,
        IAsyncBackgroundJob<EntityDeletedArgs<SpecificationAttribute>>
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;
        private readonly ILogger<ElasticCategorySyncronizationJob> _logger;

        public ElasticSpecificationAttributeSyncronizationJob(ElasticsearchClient elasticsearchClient, ILogger<ElasticCategorySyncronizationJob> logger, IObjectMapper mapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _logger = logger;
            _objectMapper = mapper;
        }

        public async Task ExecuteAsync(EntityCreatedArgs<SpecificationAttribute> args)
        {
            var elasticEntity = _objectMapper.Map<SpecificationAttribute, ElasticSpecificationAttribute>(args.Entity);

            var response =  await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task ExecuteAsync(EntityUpdatedArgs<SpecificationAttribute> args)
        {
            var elasticEntity = _objectMapper.Map<SpecificationAttribute, ElasticSpecificationAttribute>(args.Entity);

           var response = await _elasticsearchClient.UpdateAsync<ElasticSpecificationAttribute, object>(ElasticEntitiesConsts.SpecificationAttributeIndex, elasticEntity.Id, des =>
                des.Doc(elasticEntity)
            );
        }

        public async Task ExecuteAsync(EntityDeletedArgs<SpecificationAttribute> args)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticSpecificationAttribute>(args.Entity.Id);
        }
    }
}
