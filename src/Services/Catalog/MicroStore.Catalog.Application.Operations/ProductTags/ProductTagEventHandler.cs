using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Application.Operations.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
namespace MicroStore.Catalog.Application.Operations.ProductTags
{
    public class ProductTagEventHandler :
        ILocalEventHandler<EntityCreatedEventData<Tag>>,
        ILocalEventHandler<EntityUpdatedEventData<Tag>>,
        ILocalEventHandler<EntityDeletedEventData<Tag>>,
        ITransientDependency

    {
        private readonly ElasticsearchClient _elasticsearchClient;
        public ProductTagEventHandler(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }
        public async Task HandleEventAsync(EntityCreatedEventData<Tag> eventData)
        {
            var elasticTag = PrepareElasticTag(eventData.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticTag);

            response.ThrowIfFailure();
        }
        public async Task HandleEventAsync(EntityUpdatedEventData<Tag> eventData)
        {
            var elasticTag = PrepareElasticTag(eventData.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticTag);

            var productQueryDescriptor = PrepareUpdateProductQueryDescriptor(elasticTag);

            var updateResponse = await _elasticsearchClient.UpdateByQueryAsync(productQueryDescriptor);

            updateResponse.ThrowIfFailure();
        }
        public async Task HandleEventAsync(EntityDeletedEventData<Tag> eventData)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticTag>(eventData.Entity.Id);

            response.ThrowIfFailure();
        }
        private ElasticTag PrepareElasticTag(Tag tag)
        {
            var elasticTag = new ElasticTag
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description
            };

            return elasticTag;
        }

        private UpdateByQueryRequestDescriptor<ElasticProduct> PrepareUpdateProductQueryDescriptor(ElasticTag elasticEntity)
        {
            var queryDescriptor = new UpdateByQueryRequestDescriptor<ElasticProduct>(IndexName.From<ElasticProduct>())
                .Script(new Script(new InlineScript
                {
                    Source = @"
                            for(int i =0; i< ctx._source.tags.size(); i++)
                            {
                                if(ctx._source.tags[i].id == params.id)
                                {
                                    ctx._source.tags[i].name = params.name;
                                    ctx._source.tags[i].description = params.description;
                                    break;
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
                        .Match(mt => mt
                            .Field(x => x.Categories.First().Id)
                            .Query(elasticEntity.Id)
                         )
                    )
                )
            );

            return queryDescriptor;
        }
    }
}
