using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.Manufacturers
{
    public class ElasticManufacturerSyncronizationHandler :
        IConsumer<EntityCreatedEvent<ManufacturerEto>>,
        IConsumer<EntityUpdatedEvent<ManufacturerEto>>,
        IConsumer<EntityDeletedEvent<ManufacturerEto>>,
        ITransientDependency

    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IObjectMapper _objectMapper;

        public ElasticManufacturerSyncronizationHandler(ElasticsearchClient elasticsearchClient, IObjectMapper objectMapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _objectMapper = objectMapper;
        }

        public async Task Consume(ConsumeContext<EntityCreatedEvent<ManufacturerEto>> context)
        {
            var elasticEntity = _objectMapper.Map<ManufacturerEto, ElasticManufacturer>(context.Message.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task Consume(ConsumeContext<EntityUpdatedEvent<ManufacturerEto>> context)
        {
            var elasticEntity = _objectMapper.Map<ManufacturerEto, ElasticManufacturer>(context.Message.Entity);

            await _elasticsearchClient.IndexAsync(elasticEntity);

            var productQueryDescriptor = PrepareUpdateProductQueryDescriptor(elasticEntity);

            await _elasticsearchClient.UpdateByQueryAsync(productQueryDescriptor);

        }

        public async Task Consume(ConsumeContext<EntityDeletedEvent<ManufacturerEto>> context)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticManufacturer>(context.Message.Entity.Id);
        }

        private UpdateByQueryRequestDescriptor<ElasticProduct> PrepareUpdateProductQueryDescriptor(ElasticManufacturer elasticEntity)
        {
            var queryDescriptor = new UpdateByQueryRequestDescriptor<ElasticProduct>(IndexName.From<ElasticProduct>())
                .Script(new Script(new InlineScript
                {
                    Source = @"""
                            for(int i =0; i< ctx._source.productManufacturers.length; i++)
                            {
                                if(ctx._source.productManufacturers[i].id == params.id)
                                {
                                    ctx._source.productManufacturers[i].name = params.name;
                                    ctx._source.productManufacturers[i].description = params.description;
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
