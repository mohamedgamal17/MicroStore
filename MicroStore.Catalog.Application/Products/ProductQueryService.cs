﻿using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products;
using MicroStore.Catalog.Application.Extensions;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectMapping;
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

        public async Task<Result<ProductDto>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _elasticSearchClient.GetAsync<ElasticProduct>(id);

            if (!result.IsValidResponse)
            {
                return new Result<ProductDto>(new EntityNotFoundException(typeof(ElasticProduct), id));
            }

            var proudct = ObjectMapper.Map<ElasticProduct, ProductDto>(result.Source!);

            return proudct;
        }


        public async Task<Result<PagedResult<ProductDto>>> ListAsync(ProductListQueryModel queryParams, CancellationToken cancellationToken = default)
        {
            var result = await _elasticSearchClient.SearchAsync(PreapreSearchRequestDescriptor(queryParams));

            if (!result.IsValidResponse)
            {
                return new PagedResult<ProductDto>(new List<ProductDto>(), 0, 0, 0);
            }

            var paged = result.ToPagedResult(queryParams.Skip, queryParams.Length);

            var response = new PagedResult<ProductDto>
            {
                Skip = paged.Skip,
                Lenght = paged.Lenght,
                TotalCount = paged.TotalCount,
                Items = ObjectMapper.Map<List<ElasticProduct>, List<ProductDto>>(paged.Items.ToList())
            };

            return response;
        }

        public async Task<Result<List<ProductImageDto>>> ListProductImagesAsync(string productid, CancellationToken cancellationToken = default)
        {

            var response = await _elasticSearchClient.GetAsync<ElasticProduct>(productid);

            if (!response.IsValidResponse)
            {
                return new Result<List<ProductImageDto>>(new EntityNotFoundException(typeof(ElasticProduct), productid));
            }

            var product = response.Source!;

            var images = product.ProductImages;

            return ObjectMapper.Map<List<ElasticProductImage>, List<ProductImageDto>>(images);
        }

        public async Task<Result<PagedResult<ProductDto>>> SearchByImage(ProductSearchByImageQueryModel queryParams, CancellationToken cancellationToken = default)
        {
            var featureResult = await _imageService.Descripe(queryParams.Image);

            if (featureResult.IsFailure)
            {
                return new Result<PagedResult<ProductDto>>(featureResult.Exception);
            }

            var result = await _elasticSearchClient.SearchAsync<ElasticProduct>(sr => sr
                    .Knn(k => k
                        .Field(x => x.ProductImages.First().Features)
                        .QueryVector(featureResult.Value)
                        .k(100)
                        .NumCandidates(200)
                   )
                    .SourceExcludes(Infer.Fields<ElasticProduct>(
                            src=> src.ProductImages.First().Features
                        )
                   )
                 .Size(queryParams.Length)
                 .From(queryParams.Skip)
                );


            if (!result.IsValidResponse)
            {
                return new PagedResult<ProductDto>(new List<ProductDto>(), 0, 0, 0);
            }

            var paged = result.ToPagedResult(queryParams.Skip,queryParams.Length);

            var response = new PagedResult<ProductDto>
            {
                Skip = paged.Skip,
                Lenght = paged.Lenght,
                TotalCount = paged.TotalCount,
                Items = ObjectMapper.Map<List<ElasticProduct>, List<ProductDto>>(paged.Items.ToList())
            };

            return response;
        }



        public async Task<Result<PagedResult<ProductDto>>> GetUserRecommendation(string userId,
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
                .SourceExcludes(Infer.Fields<ElasticProduct>(
                    src => src.ProductImages.First().Features
                  )
                )
                .Size(pagingParams.Length)
            );

            var paged = productsResponse.ToPagedResult(pagingParams.Skip, pagingParams.Length);

            var response = new PagedResult<ProductDto>
            {
                Skip = paged.Skip,
                Lenght = paged.Lenght,
                TotalCount = paged.TotalCount,
                Items = ObjectMapper.Map<List<ElasticProduct>, List<ProductDto>>(paged.Items.ToList())
            };

            return response;
        }


        public async Task<Result<PagedResult<ProductDto>>> GetSimilarItems(string productId, PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var result = await _elasticSearchClient.SearchAsync(PreapreSimilarItemsSearchQuery(productId, queryParams.Skip, queryParams.Length));

            if (!result.IsValidResponse)
            {
                return new PagedResult<ProductDto>(new List<ProductDto>(), 0, 0, 0);
            }

            var paged = result.ToPagedResult(queryParams.Skip, queryParams.Length);

            var response = new PagedResult<ProductDto>
            {
                Skip = paged.Skip,
                Lenght = paged.Lenght,
                TotalCount = paged.TotalCount,
                Items = ObjectMapper.Map<List<ElasticProduct>, List<ProductDto>>(paged.Items.ToList())
            };

            return response;
        }



        private SearchRequestDescriptor<ElasticProduct> PreapreSearchRequestDescriptor(ProductListQueryModel queryParams)
        {

            return new SearchRequestDescriptor<ElasticProduct>()
                .Query(qr => qr
                    .Bool(bl => bl
                        .Must(mt => mt
                            .When(!string.IsNullOrEmpty(queryParams.Name), act => act
                                .MatchPhrasePrefix(mt => mt
                                    .Field(x => x.Name)
                                    .Query(queryParams.Name!)
                                )
                            )
                        )
                        .Filter(flt => flt
                            .When(!string.IsNullOrEmpty(queryParams.Category), act => act
                                .Nested(cf=> cf.Path(p=> p.Categories).Query(nqr=> nqr
                                    .Term(x=> x.Categories.First().Name,queryParams.Category!)
                                ))
                            )
                            .When(!string.IsNullOrEmpty(queryParams.Manufacturer), act => act
                                .Nested(cf => cf.Path(p => p.Manufacturers).Query(nqr => nqr
                                    .Term(x => x.Manufacturers.First().Name, queryParams.Manufacturer!)
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
                .SourceExcludes(Infer.Fields<ElasticProduct>(
                    src => src.ProductImages.First().Features
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
                            case "creation":  srt.Field(x => x.CreationTime, cfg => cfg.Order(queryParams.Desc ? SortOrder.Desc : SortOrder.Asc));
                                break;
                            default:
                                srt.Field(x => x.CreationTime, cfg => cfg.Order(queryParams.Desc ? SortOrder.Desc : SortOrder.Asc));
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
               .SourceExcludes(Infer.Fields<ElasticProduct>(
                    src => src.ProductImages.First().Features
                  )
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
                                    x => x.Categories.First().Name,
                                    x => x.Manufacturers.First().Name,
                                    x => x.Tags.First().Name
                               )
                         )
                        .Like(new List<Like> { new Like(new LikeDocument() { Id = productId }) })
                        .MinTermFreq(1)
                        .MaxQueryTerms(20)
                 )
              )
              .SourceExcludes(Infer.Fields<ElasticProduct>(
                    src=> src.ProductImages.First().Features
                  )
              )
              .TrackTotalHits(new TrackHits(true))
              .Size(length)
              .From(skip);

        }

        public async Task<Result<ProductImageDto>> GetProductImageAsync(string productId, string imageId, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.GetAsync<ElasticProduct>(productId);

            if (!response.IsValidResponse)
            {
                return new Result<ProductImageDto>(new EntityNotFoundException(typeof(ElasticProduct), productId));
            }

            var product = response.Source!;

            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == imageId);

            if(productImage == null)
            {
                return new Result<ProductImageDto>(new EntityNotFoundException(typeof(ElasticProductImage)));
            }

           return ObjectMapper.Map<ElasticProductImage, ProductImageDto>(productImage);
        }
    }


}
