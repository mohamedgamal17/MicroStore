
using Elastic.Clients.Elasticsearch;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Operations.Manufacturers
{
    public class ElasticManufacturerSyncronizationJob :
        IAsyncBackgroundJob<EntityCreatedArgs<Manufacturer>>,
        IAsyncBackgroundJob<EntityUpdatedArgs<Manufacturer>>,
        IAsyncBackgroundJob<EntityDeletedArgs<Manufacturer>>

    {

        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;

        public ElasticManufacturerSyncronizationJob(ElasticsearchClient elasticsearchClient, IObjectMapper objectMapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _objectMapper = objectMapper;
        }

        public async Task ExecuteAsync(EntityCreatedArgs<Manufacturer> args)
        {
            var elasticEntity = _objectMapper.Map<Manufacturer, ElasticManufacturer>(args.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task ExecuteAsync(EntityUpdatedArgs<Manufacturer> args)
        {
            var elasticEntity = _objectMapper.Map<Manufacturer, ElasticManufacturer>(args.Entity);

            var response = await _elasticsearchClient.UpdateAsync<ElasticManufacturer, object>(ElasticEntitiesConsts.ManufacturerIndex, elasticEntity.Id, des =>
                 des.Doc(elasticEntity)
             );
        }

        public async Task ExecuteAsync(EntityDeletedArgs<Manufacturer> args)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticManufacturer>(args.Entity.Id);
        }
    }
}
