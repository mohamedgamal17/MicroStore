using Elastic.Clients.Elasticsearch;
using Hangfire;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.Categories
{
    public class ElasticCategorySyncronizationJob :
        IAsyncBackgroundJob<EntityCreatedArgs<Category>>,
        IAsyncBackgroundJob<EntityUpdatedArgs<Category>>,
        IAsyncBackgroundJob<EntityDeletedArgs<Category>>
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;
        private readonly ILogger<ElasticCategorySyncronizationJob> _logger;

        public ElasticCategorySyncronizationJob(ElasticsearchClient elasticsearchClient, ILogger<ElasticCategorySyncronizationJob> logger, IObjectMapper mapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _logger = logger;
            _objectMapper = mapper;
        }

        public async Task ExecuteAsync(EntityCreatedArgs<Category> args)
        {
            var elasticEntity = _objectMapper.Map<Category, ElasticCategory>(args.Entity);

            var response =  await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task ExecuteAsync(EntityUpdatedArgs<Category> args)
        {
            var elasticEntity = _objectMapper.Map<Category, ElasticCategory>(args.Entity);

           var response = await _elasticsearchClient.UpdateAsync<ElasticCategory, object>(ElasticEntitiesConsts.CategoryIndex, elasticEntity.Id, des =>
                des.Doc(elasticEntity)
            );
        }

        public async Task ExecuteAsync(EntityDeletedArgs<Category> args)
        {
            var response = await _elasticsearchClient.DeleteAsync<Category>(args.Entity.Id);
        }
    }
}
