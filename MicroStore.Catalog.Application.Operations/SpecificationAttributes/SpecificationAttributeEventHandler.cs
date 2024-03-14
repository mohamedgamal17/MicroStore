using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Application.Operations.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
namespace MicroStore.Catalog.Application.Operations.SpecificationAttributes
{
    public class SpecificationAttributeEventHandler :
        ILocalEventHandler<EntityCreatedEventData<SpecificationAttribute>>,
        ILocalEventHandler<EntityUpdatedEventData<SpecificationAttribute>>,
        ILocalEventHandler<EntityDeletedEventData<SpecificationAttribute>>,
        ITransientDependency

    {

        private readonly ElasticsearchClient _elasticsearchClient;

        public SpecificationAttributeEventHandler(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<SpecificationAttribute> eventData)
        {
            var elasticEntity = PrepareElasticSpecificationAttribute(eventData.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);

            response.ThrowIfFailure();
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<SpecificationAttribute> eventData)
        {
            var elasticEntity = PrepareElasticSpecificationAttribute(eventData.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticEntity);

            response.ThrowIfFailure();
        }

        public async Task HandleEventAsync(EntityDeletedEventData<SpecificationAttribute> eventData)
        {
            var response = await _elasticsearchClient.DeleteAsync<ElasticSpecificationAttribute>(eventData.Entity.Id);

            response.ThrowIfFailure();
        }

        private ElasticSpecificationAttribute PrepareElasticSpecificationAttribute(SpecificationAttribute specificationAttribute)
        {
            var elasticEntity = new ElasticSpecificationAttribute
            {
                Id = specificationAttribute.Id,
                Name = specificationAttribute.Name,
                Description = specificationAttribute.Description,
                Options = specificationAttribute.Options?.Select(x => new ElasticSpecificationAttributeOption
                {
                    Id = x.Id,
                    Value = x.Name,
                }).ToList()

            };

            return elasticEntity;
        }
    }
}
