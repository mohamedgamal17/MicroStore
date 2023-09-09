using Elastic.Clients.Elasticsearch;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Extensions;
using MicroStore.Catalog.Application.Models.Manufacturers;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Application.Manufacturers
{
    public class ManufacturerQueryService : CatalogApplicationService, IManufacturerQueryService
    {
        private readonly ElasticsearchClient _elasticSearchClient;
        public ManufacturerQueryService( ElasticsearchClient elasticSearchClient)
        {
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task<Result<ElasticManufacturer>> GetAsync(string manufacturerId, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.GetAsync<ElasticManufacturer>(manufacturerId,cancellationToken);

            if (!response.IsValidResponse)
            {
                return new Result<ElasticManufacturer>(new EntityNotFoundException(typeof(ElasticManufacturer), manufacturerId));
            }

            return response.Source!;
        }

        public async Task<Result<List<ElasticManufacturer>>> ListAsync(ManufacturerListQueryModel queryParams, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.SearchAsync(PreapreSearchRequestDescriptor(queryParams));

            if (!response.IsValidResponse)
            {
                return new List<ElasticManufacturer>();
            }

            return response.Documents.ToList();     
        }
        private SearchRequestDescriptor<ElasticManufacturer> PreapreSearchRequestDescriptor(ManufacturerListQueryModel queryParams)
        {
            return new SearchRequestDescriptor<ElasticManufacturer>()
                .Query(desc => desc
                    .Bool(bl => bl
                        .Must(mst => mst
                            .When(!string.IsNullOrEmpty(queryParams.Name), act => act
                                .MatchPhrase(mt => mt
                                    .Field(x => x.Name)
                                    .Query(queryParams.Name!)
                                )
                            )
                        )
                    )
                )
                .Size(1000)
                .When(!string.IsNullOrEmpty(queryParams.SortBy), act => act
                    .Sort(srt => srt
                        .When(queryParams.Name == "name", act => act
                            .Field(x => x.Name, cfg => cfg.Order(queryParams.Desc ? SortOrder.Desc : SortOrder.Asc))
                        )
                    )
                );
        }
    }
}
