using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Operations.Products
{
    public class ElasticProductSynchronizationHandler :
        IConsumer<EntityCreatedEvent<ProductEto>>,
        IConsumer<EntityUpdatedEvent<ProductEto>>,
        ITransientDependency
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;

        public ElasticProductSynchronizationHandler(ElasticsearchClient elasticsearchClient, IObjectMapper objectMapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _objectMapper = objectMapper;
        }

        public async Task Consume(ConsumeContext<EntityCreatedEvent<ProductEto>> context)
        {
            var elasticEntity = _objectMapper.Map<ProductEto, ElasticProduct>(context.Message.Entity);

            var elasticProductVector = new ElasticImageVector
            {
                Id = context.Message.Entity.Id,
                ProductId = context.Message.Entity.Id
            };

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);

            await _elasticsearchClient.IndexAsync(elasticProductVector);
        }

        public async Task Consume(ConsumeContext<EntityUpdatedEvent<ProductEto>> context)
        {
            var elasticEntity = _objectMapper.Map<ProductEto, ElasticProduct>(context.Message.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }



    }
}
