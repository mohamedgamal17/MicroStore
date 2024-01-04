using Elastic.Clients.Elasticsearch;
using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Catalog.Application.Operations.Categories
{
    public class ElasticCategorySyncronizationHandler :
        IConsumer<EntityCreatedEvent<CategoryEto>>,
        IConsumer<EntityUpdatedEvent<CategoryEto>>,
        IConsumer<EntityDeletedEvent<CategoryEto>>,
        ITransientDependency
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        
        private readonly IObjectMapper _objectMapper;
        private readonly ILogger<ElasticCategorySyncronizationHandler> _logger;

        public ElasticCategorySyncronizationHandler(ElasticsearchClient elasticsearchClient, ILogger<ElasticCategorySyncronizationHandler> logger, IObjectMapper mapper)
        {
            _elasticsearchClient = elasticsearchClient;
            _logger = logger;
            _objectMapper = mapper;
        }

        public async Task Consume(ConsumeContext<EntityCreatedEvent<CategoryEto>> context)
        {
            var elasticEntity = _objectMapper.Map<CategoryEto, ElasticCategory>(context.Message.Entity);

            await _elasticsearchClient.IndexAsync(elasticEntity);
        }

        public async Task Consume(ConsumeContext<EntityUpdatedEvent<CategoryEto>> context)
        {
            var elasticEntity = _objectMapper.Map<CategoryEto, ElasticCategory>(context.Message.Entity);

            await _elasticsearchClient.IndexAsync(elasticEntity);

            var productQueryDescriptor = PrepareUpdateProductQueryDescriptor(elasticEntity);

            var response  =  await _elasticsearchClient.UpdateByQueryAsync(productQueryDescriptor);

            _logger.LogInformation("Complete Synchronizing Category After Update");
            _logger.LogInformation(response.DebugInformation);


        }
        public async Task Consume(ConsumeContext<EntityDeletedEvent<CategoryEto>> context)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticCategory>(context.Message.Entity.Id);
        }


        private UpdateByQueryRequestDescriptor<ElasticProduct> PrepareUpdateProductQueryDescriptor(ElasticCategory elasticEntity)
        {
            var queryDescriptor = new UpdateByQueryRequestDescriptor<ElasticProduct>(IndexName.From<ElasticProduct>())
                .Script(new Script(new InlineScript
                {
                    Source = @" 
                        for(int i = 0; i < ctx._source.categories.size(); i++){
                          if(ctx._source.categories[i].id == params.id){
                                ctx._source.categories[i].name =params.name;
                                ctx._source.categories[i].description= params.description;
                           }
                         }
                        ",
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
                    .Path(pt => pt.Categories)
                    .Query(qr => qr
                        .Term(mt=> mt.Categories.First().Id , elasticEntity.Id)
                    )
                )
            );

            return queryDescriptor;
        }

    }
}
