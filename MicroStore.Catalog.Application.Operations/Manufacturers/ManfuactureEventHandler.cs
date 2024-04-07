using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Application.Operations.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
namespace MicroStore.Catalog.Application.Operations.Manufacturers
{
    public class ManfuactureEventHandler : 
        ILocalEventHandler<EntityCreatedEventData<Manufacturer>>,
        ILocalEventHandler<EntityUpdatedEventData<Manufacturer>>,
        ILocalEventHandler<EntityDeletedEventData<Manufacturer>>,
        ITransientDependency
    {

        private readonly ElasticsearchClient _elasticsearchClient;

        public ManfuactureEventHandler(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<Manufacturer> eventData)
        {
            var elasticManufacturer = PrepareElasticManufacturer(eventData.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticManufacturer);

            response.ThrowIfFailure();
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Manufacturer> eventData)
        {
            var elasticManufacturer = PrepareElasticManufacturer(eventData.Entity);

            var indexResponse = await _elasticsearchClient.IndexAsync(elasticManufacturer);

            indexResponse.ThrowIfFailure();

            var productQueryDescriptor = PrepareUpdateProductQueryDescriptor(elasticManufacturer);

            var updateResponse = await _elasticsearchClient.UpdateByQueryAsync(productQueryDescriptor);

            updateResponse.ThrowIfFailure();
        }

        public async Task HandleEventAsync(EntityDeletedEventData<Manufacturer> eventData)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticManufacturer>(eventData.Entity.Id);

            response.ThrowIfFailure();
        }

        private ElasticManufacturer PrepareElasticManufacturer(Manufacturer manufacturer)
        {
            var elasticManufacturer = new ElasticManufacturer
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Description = manufacturer.Description,
                CreationTime = manufacturer.CreationTime,
                CreatorId = manufacturer.CreatorId?.ToString(),
                LastModifierId = manufacturer.LastModifierId?.ToString(),
                LastModificationTime = manufacturer.LastModificationTime
            };

            return elasticManufacturer;
        }

        private UpdateByQueryRequestDescriptor<ElasticProduct> PrepareUpdateProductQueryDescriptor(ElasticManufacturer elasticEntity)
        {
            var queryDescriptor = new UpdateByQueryRequestDescriptor<ElasticProduct>(IndexName.From<ElasticProduct>())
                .Script(new Script(new InlineScript
                {
                    Source = @"
                            for(int i =0; i< ctx._source.manufacturers.size(); i++)
                            {
                                if(ctx._source.manufacturers[i].id == params.id)
                                {
                                    ctx._source.manufacturers[i].name = params.name;
                                    ctx._source.manufacturers[i].description = params.description;
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
                    .Path(pt => pt.Manufacturers)
                    .Query(qr => qr
                        .Term(mt => mt.Manufacturers.First().Id, elasticEntity.Id)
                    )
                )
            );

            return queryDescriptor;
        }
    }
}
