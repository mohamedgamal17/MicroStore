using Elastic.Clients.Elasticsearch;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Abstractions.Categories;
using MicroStore.Catalog.Application.Extensions;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Application.Categories
{
    public class CategoryQueryService : CatalogApplicationService, ICategoryQueryService
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        public CategoryQueryService(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task<Result<CategoryDto>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var response = await _elasticsearchClient.GetAsync<ElasticCategory>(id);

            if (!response.IsValidResponse)
            {
                return new Result<CategoryDto>(new EntityNotFoundException(typeof(ElasticCategory), id));
            }

            var result = response.Source!;

            return ObjectMapper.Map<ElasticCategory, CategoryDto>(result);
        }

        public async Task<Result<List<CategoryDto>>> ListAsync(CategoryListQueryModel queryParams, CancellationToken cancellationToken = default)
        {
            var response = await _elasticsearchClient.SearchAsync(PreapreSearchRequestDescriptor(queryParams));

            if(!response.IsValidResponse)
            {
                return new List<CategoryDto>();
            }

            var result =  response.Documents.ToList() ;

            return ObjectMapper.Map<List<ElasticCategory>, List<CategoryDto>>(result);
        }


        private SearchRequestDescriptor<ElasticCategory> PreapreSearchRequestDescriptor(CategoryListQueryModel queryParams)
        {
            return new SearchRequestDescriptor<ElasticCategory>()
                .Query(desc => desc
                    .Bool(bl => bl
                        .Must(mt => mt
                            .When(!string.IsNullOrEmpty(queryParams.Name), act => act
                                .MatchPhrasePrefix(mat => mat
                                    .Field(x => x.Name)
                                    .Query(queryParams.Name!)
                                )
                            )
                        )
                    )
                )
                .Size(1000)
                .When(queryParams.SortBy != null, act=> act
                    .Sort(srt => srt
                        .When(queryParams.SortBy!.ToLower() == "name", act => act
                            .Field(x => x.Name, cfg => cfg.Order(queryParams.Desc ? SortOrder.Desc : SortOrder.Asc))
                        )

                    )            
                );
        }
    }



}
