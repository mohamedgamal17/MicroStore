using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Application.Operations.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
namespace MicroStore.Catalog.Application.Operations.Categories
{
    public class CategoryEventHandler :
        ILocalEventHandler<EntityCreatedEventData<Category>>,
        ILocalEventHandler<EntityUpdatedEventData<Category>>,
        ILocalEventHandler<EntityDeletedEventData<Category>>,
        ITransientDependency
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        public CategoryEventHandler(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<Category> eventData)
        {
            var elasticEntity = PrepareElasticCategory(eventData.Entity);

            await _elasticsearchClient.IndexAsync(elasticEntity);

        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Category> eventData)
        {
            var elasticEntity = PrepareElasticCategory(eventData.Entity);

            var indexResponse = await _elasticsearchClient.IndexAsync(elasticEntity);

            indexResponse.ThrowIfFailure();

            var productQueryDescriptor = PrepareUpdateProductQueryDescriptor(elasticEntity);

            var updateResponse = await _elasticsearchClient.UpdateByQueryAsync(productQueryDescriptor);

            updateResponse.ThrowIfFailure();

        }

        public async Task HandleEventAsync(EntityDeletedEventData<Category> eventData)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticCategory>(eventData.Entity.Id);

            response.ThrowIfFailure();
        }


        private ElasticCategory PrepareElasticCategory(Category category)
        {
            var elasticCategory = new ElasticCategory
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreationTime = category.CreationTime,
                CreatorId = category.CreatorId?.ToString(),
                LastModifierId = category.LastModifierId?.ToString(),
                LastModificationTime = category.LastModificationTime,
            };

            return elasticCategory;
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
                        .Term(mt => mt.Categories.First().Id, elasticEntity.Id)
                    )
                )
            );

            return queryDescriptor;
        }

    }
}
