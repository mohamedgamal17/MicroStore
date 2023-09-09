using Elastic.Clients.Elasticsearch;
using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Application.Operations.Categories;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.SpecificationAttributes
{
    public class ElasticSpecificationAttributeSyncronizationJob :
        IConsumer<EntityCreatedEvent<SpecificationAttributeEto>>,
        IConsumer<EntityUpdatedEvent<SpecificationAttributeEto>>,
        IConsumer<EntityDeletedEvent<SpecificationAttributeEto>>,
        ITransientDependency
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;
        private readonly ILogger<ElasticCategorySyncronizationHandler> _logger;

        public ElasticSpecificationAttributeSyncronizationJob(ElasticsearchClient elasticsearchClient, ILogger<ElasticCategorySyncronizationHandler> logger, IObjectMapper mapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _logger = logger;
            _objectMapper = mapper;
        }

        public async Task Consume(ConsumeContext<EntityCreatedEvent<SpecificationAttributeEto>> context)
        {
            var elasticEntity = _objectMapper
                .Map<SpecificationAttributeEto, ElasticSpecificationAttribute>(context.Message.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task Consume(ConsumeContext<EntityUpdatedEvent<SpecificationAttributeEto>> context)
        {
            var elasticEntity = _objectMapper
                .Map<SpecificationAttributeEto, ElasticSpecificationAttribute>(context.Message.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task Consume(ConsumeContext<EntityDeletedEvent<SpecificationAttributeEto>> context)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticSpecificationAttribute>(context.Message.Entity.Id);

        }


    }
}
