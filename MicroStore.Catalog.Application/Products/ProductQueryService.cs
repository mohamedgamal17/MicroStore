using AutoMapper.QueryableExtensions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Extensions;
using MicroStore.Catalog.Application.Models.Products;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;
namespace MicroStore.Catalog.Application.Products
{
    [DisableValidation]
    public class ProductQueryService : CatalogApplicationService, IProductQueryService
    {
        private readonly ICatalogDbContext _catalogDbContext;

        private readonly IImageService _imageService;

        private readonly ElasticsearchClient _elasticSearchClient;
        public ProductQueryService(ICatalogDbContext catalogDbContext, IImageService imageService, ElasticsearchClient elasticSearchClient)
        {
            _catalogDbContext = catalogDbContext;
            _imageService = imageService;
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task<Result<ElasticProduct>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.GetAsync<ElasticProduct>(id);

            if (!response.IsValidResponse)
            {
                return new Result<ElasticProduct>(new EntityNotFoundException(typeof(ElasticProduct), id));
            }

            return response.Source!;
        }


        public async Task<Result<PagedResult<ElasticProduct>>> ListAsync(ProductListQueryModel queryParams, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.SearchAsync(PreapreSearchRequestDescriptor(queryParams));

            if (!response.IsValidResponse)
            {
                return new PagedResult<ElasticProduct>(new List<ElasticProduct>(), 0, 0, 0);
            }

            return response.ToPagedResult(queryParams.Skip, queryParams.Length);
        }

        public async Task<Result<List<ElasticProductImage>>> ListProductImagesAsync(string productid, CancellationToken cancellationToken = default)
        {

            var response = await _elasticSearchClient.GetAsync<ElasticProduct>(productid);

            if (!response.IsValidResponse)
            {
                return new Result<List<ElasticProductImage>>(new EntityNotFoundException(typeof(ElasticProduct), productid));
            }

            var product = response.Source!;

            return product.ProductImages;
        }

        public async Task<Result<List<ProductDto>>> SearchByImage(ProductSearchByImageModel model, CancellationToken cancellationToken = default)
        {
            var relatedImages = await _imageService.SearchByImage(model.Image);

            var query = _catalogDbContext.Products
                .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider)
                .AsQueryable();

            query = from pr in query
                           where relatedImages.Select(x => x.ProductId).Contains(pr.Id)
                           select pr;

            var products = await query.ToListAsync();

            return products.OrderBy(x=>  relatedImages.FindIndex(c=> c.Id == x.Id)).ToList();
        }



        public async Task<Result<PagedResult<ElasticProduct>>> GetUserRecommendation(string userId,
            PagingQueryParams pagingParams, CancellationToken cancellationToken)
        {
            var (expectedProducts, count) = await GetUserRecommandedProduct(userId, pagingParams.Skip, pagingParams.Length);

            var productIds = expectedProducts?.Select(x => FieldValue.String(x.Id)).ToList();


            var productsResponse = await _elasticSearchClient.SearchAsync<ElasticProduct>(desc => desc
                .Query(qr => qr
                    .Terms(tr => tr
                    .Field(x => x.Id)
                    .Terms(new Elastic.Clients.Elasticsearch.QueryDsl.TermsQueryField(productIds))
                    )
                )
                .Size(pagingParams.Length)
            );


            return new PagedResult<ElasticProduct>(productsResponse.Documents, count, pagingParams.Skip, pagingParams.Length);

        }


        public async Task<Result<PagedResult<ElasticProduct>>> GetSimilarItems(string productId, PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.SearchAsync(PreapreSimilarItemsSearchQuery(productId, queryParams.Skip, queryParams.Length));

            if (!response.IsValidResponse)
            {
                return new PagedResult<ElasticProduct>(new List<ElasticProduct>(), 0, 0, 0);
            }

            return response.ToPagedResult(queryParams.Skip, queryParams.Length);
        }



        private SearchRequestDescriptor<ElasticProduct> PreapreSearchRequestDescriptor(ProductListQueryModel queryParams)
        {

            return new SearchRequestDescriptor<ElasticProduct>()
                .Query(qr => qr
                    .Bool(bl => bl
                        .Must(mt => mt
                            .When(queryParams.Name != null, act => act
                                .MatchPhrasePrefix(mt => mt
                                    .Field(x => x.Name)
                                    .Query(queryParams.Name!)
                                )
                            )
                        )
                        .Filter(flt => flt
                            .When(queryParams.Category != null, act => act
                                .Nested(cf=> cf.Path(p=> p.ProductCategories).Query(nqr=> nqr
                                    .Term(x=> x.ProductCategories.First().Name,queryParams.Category!)
                                ))
                            )
                            .When(queryParams.Manufacturer != null, act => act
                                .Nested(cf => cf.Path(p => p.ProductManufacturers).Query(nqr => nqr
                                    .Term(x => x.ProductManufacturers.First().Name, queryParams.Manufacturer!)
                                ))
                            )
                            .When(queryParams.MinPrice != null || queryParams.MaxPrice != null, act => act
                                .Range(rng => rng
                                    .NumberRange(numrang => numrang
                                        .When(queryParams.MinPrice != null, act =>
                                            act.Field(x => x.Price).Gte(queryParams.MinPrice!)
                                        )
                                        .When(queryParams.MaxPrice != null, act =>
                                            act.Field(x => x.Price).Lte(queryParams.MaxPrice!)
                                        )
                                    )
                                )

                            ).When(queryParams.IsFeatured, act => act
                                .Term(x => x.IsFeatured, true)
                            )
                        )
                    )

                )
                .TrackTotalHits(new TrackHits(true))
                .Size(queryParams.Length)
                .From(queryParams.Skip)
                .When(queryParams.SortBy != null, act => act
                    .Sort(srt =>
                    {
                        switch (queryParams.SortBy!.ToLower())
                        {
                            case "name":
                                srt.Field(x => x.Name, cfg => cfg.Order(queryParams.Desc ? SortOrder.Desc : SortOrder.Asc));
                                break;
                            case "price":
                                srt.Field(x => x.Name, cfg => cfg.Order(queryParams.Desc ? SortOrder.Desc : SortOrder.Asc));
                                break;
                            case "creation":  srt.Field(x => x.CreatationTime, cfg => cfg.Order(queryParams.Desc ? SortOrder.Desc : SortOrder.Asc));
                                break;
                            default:
                                srt.Field(x => x.CreatationTime, cfg => cfg.Order(queryParams.Desc ? SortOrder.Desc : SortOrder.Asc));
                                break;


                        }

                    })

                );

        }

        private async Task<(IEnumerable<ElasticProductExpectedRating>, long)> GetUserRecommandedProduct(string userId, int skip, int length)
        {
            var productRatingResponse = await _elasticSearchClient.SearchAsync<ElasticProductExpectedRating>(desc => desc
               .Query(q => q
                   .Match(mt => mt
                       .Field(x => x.UserId)
                       .Query(userId)
                   )
               )
               .Sort(srt => srt
                   .Field(x => x.Score, cfg => cfg.Order(SortOrder.Desc))

               )
               .TrackTotalHits(new TrackHits(true))
               .From(skip)
               .Size(length)
            );




            return (productRatingResponse.Documents, productRatingResponse.Total);
        }

        private SearchRequestDescriptor<ElasticProduct> PreapreSimilarItemsSearchQuery(string productId, int skip, int length)
        {
            return new SearchRequestDescriptor<ElasticProduct>()
                .Query(qr => qr
                    .MoreLikeThis(mr => mr
                        .Fields(
                            Infer.Fields<ElasticProduct>(
                                    x => x.Name,
                                    x => x.ShortDescription,
                                    x => x.ProductCategories.First().Name,
                                    x => x.ProductManufacturers.First().Name,
                                    x => x.ProductTags.First().Name
                               )
                         )
                        .Like(new List<Like> { new Like(new LikeDocument() { Id = productId }) })
                        .MinTermFreq(1)
                        .MaxQueryTerms(20)
                 )
              )
              .TrackTotalHits(new TrackHits(true))
              .Size(length)
              .From(skip);

        }

        public async Task<Result<ElasticProductImage>> GetProductImageAsync(string productId, string imageId, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.GetAsync<ElasticProduct>(productId);

            if (!response.IsValidResponse)
            {
                return new Result<ElasticProductImage>(new EntityNotFoundException(typeof(ElasticProduct), productId));
            }

            var product = response.Source!;

            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == imageId);

            if(productImage == null)
            {
                return new Result<ElasticProductImage>(new EntityNotFoundException(typeof(ElasticProductImage)));
            }

            return productImage;
        }
    }


}
