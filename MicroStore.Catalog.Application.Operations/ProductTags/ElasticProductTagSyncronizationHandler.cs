using Elastic.Clients.Elasticsearch;
using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Application.Operations.Categories;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.ProductTags
{
    public class ElasticProductTagSyncronizationHandler :
        IConsumer<EntityCreatedEvent<ProductTagEto>>,
        IConsumer<EntityUpdatedEvent<ProductTagEto>>,
        IConsumer<EntityDeletedEvent<ProductTagEto>>,
        ITransientDependency
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;
        private readonly ILogger<ElasticCategorySyncronizationHandler> _logger;

        public ElasticProductTagSyncronizationHandler(ElasticsearchClient elasticsearchClient, ILogger<ElasticCategorySyncronizationHandler> logger, IObjectMapper mapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _logger = logger;
            _objectMapper = mapper;
        }

        public async Task Consume(ConsumeContext<EntityCreatedEvent<ProductTagEto>> context)
        {
            var elasticEntity = _objectMapper.Map<ProductTagEto, ElasticProductTag>(context.Message.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task Consume(ConsumeContext<EntityUpdatedEvent<ProductTagEto>> context)
        {
            var elasticEntity = _objectMapper.Map<ProductTagEto, ElasticProductTag>(context.Message.Entity);

             await _elasticsearchClient.IndexAsync<ElasticProductTag>(elasticEntity);

            var productQueryDescriptor = PrepareUpdateProductQueryDescriptor(elasticEntity);

            await _elasticsearchClient.UpdateByQueryAsync(productQueryDescriptor);
        }

        public async Task Consume(ConsumeContext<EntityDeletedEvent<ProductTagEto>> context)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticProductTag>(context.Message.Entity.Id);
        }

        private UpdateByQueryRequestDescriptor<ElasticProduct> PrepareUpdateProductQueryDescriptor(ElasticProductTag elasticEntity)
        {
            var queryDescriptor = new UpdateByQueryRequestDescriptor<ElasticProduct>(IndexName.From<ElasticProduct>())
                .Script(new Script(new InlineScript
                {
                    Source = @"""
                            for(int i =0; i< ctx._source.productTags.length; i++)
                            {
                                if(ctx._source.productTags[i].id == params.id)
                                {
                                    ctx._source.productTags[i].name = params.name;
                                    ctx._source.productTags[i].description = params.description;
                                    break;
                                }
                            }
                        """,
                    Params = new Dictionary<string, object>()
                    {
                        {"id",elasticEntity.Id },
                        {"name",elasticEntity.Name },
                        {"description",elasticEntity.Description }
                    },

                    Language = new ScriptLanguage("painless")
                }))
                .Query(desc => desc
                    .Nested(nes => nes
                    .Path(pt => pt.ProductCategories)
                    .Query(qr => qr
                        .Match(mt => mt
                            .Field(x => x.ProductCategories.First().Id)
                            .Query(elasticEntity.Id)
                         )
                    )
                )
            );

            return queryDescriptor;
        }
    }
}
