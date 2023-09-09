using AutoMapper.QueryableExtensions;
using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Extensions;
using MicroStore.Catalog.Application.Models.ProductReviews;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.ProductReviews
{
    public class ProductReviewService : CatalogApplicationService, IProductReviewService
    {
        private readonly IRepository<ProductReview> _productReviewRepository;

        private readonly IRepository<Product> _productRepository;

        private readonly ICatalogDbContext _catalogDbContext;

        private readonly ElasticsearchClient _elasticSearchClient;
        public ProductReviewService(IRepository<ProductReview> productReviewRepository, IRepository<Product> productRepository, ICatalogDbContext catalogDbContext, ElasticsearchClient elasticSearchClient)
        {
            _productReviewRepository = productReviewRepository;
            _productRepository = productRepository;
            _catalogDbContext = catalogDbContext;
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task<Result<ProductReviewDto>> CreateAsync(string productId, CreateProductReviewModel model, CancellationToken cancellationToken = default)
        {
            if (!await CheckProductExist(productId, cancellationToken))
            {
                return new Result<ProductReviewDto>(new EntityNotFoundException(typeof(Product), productId));
            }

            var productReview = new ProductReview
            {
                Title = model.Title,
                ReviewText = model.ReviewText,
                Rating = model.Rating,
                ProductId = productId,
                UserId = model.UserId
            };

            await _productReviewRepository.InsertAsync(productReview, cancellationToken: cancellationToken);


            return ObjectMapper.Map<ProductReview, ProductReviewDto>(productReview);
        }
        public async Task<Result<ProductReviewDto>> UpdateAsync(string productId, string productReviewId, ProductReviewModel model, CancellationToken cancellationToken = default)
        {
            if (!await CheckProductExist(productId, cancellationToken))
            {
                return new Result<ProductReviewDto>(new EntityNotFoundException(typeof(Product), productId));
            }

            var productReview = await _productReviewRepository.SingleOrDefaultAsync(x => x.ProductId == productId && x.Id == productReviewId, cancellationToken);

            if (productReview == null)
            {
                return new Result<ProductReviewDto>(new EntityNotFoundException(typeof(ProductReview), productReviewId));
            }

            productReview.Update(model.Title, model.ReviewText, model.Rating);

            await _productReviewRepository.UpdateAsync(productReview, cancellationToken: cancellationToken);

            return ObjectMapper.Map<ProductReview, ProductReviewDto>(productReview);
        }


        public async Task<Result<ProductReviewDto>> ReplayAsync(string productId, string productReviewId, ProductReviewReplayModel model, CancellationToken cancellationToken = default)
        {
            if (!await CheckProductExist(productId, cancellationToken))
            {
                return new Result<ProductReviewDto>(new EntityNotFoundException(typeof(Product), productId));
            }

            var productReview = await _productReviewRepository.SingleOrDefaultAsync(x => x.ProductId == productId && x.Id == productReviewId, cancellationToken);

            if (productReview == null)
            {
                return new Result<ProductReviewDto>(new EntityNotFoundException(typeof(ProductReview), productReviewId));
            }

            productReview.Replay(model.ReplayText);

            await _productReviewRepository.UpdateAsync(productReview, cancellationToken: cancellationToken);

            return ObjectMapper.Map<ProductReview, ProductReviewDto>(productReview);

        }

        public async Task<Result<Unit>> DeleteAsync(string productId, string productReviewId, CancellationToken cancellationToken = default)
        {
            if (!await CheckProductExist(productId, cancellationToken))
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(Product), productId));
            }

            var productReview = await _productReviewRepository.SingleOrDefaultAsync(x => x.ProductId == productId && x.Id == productReviewId, cancellationToken);

            if (productReview == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(ProductReview), productReviewId));
            }

            await _productReviewRepository.DeleteAsync(productReview, cancellationToken: cancellationToken);

            return Unit.Value;
        }

        private async Task<bool> CheckProductExist(string productId, CancellationToken cancellationToken)
        {
            return await _productRepository.AnyAsync(x => x.Id == productId, cancellationToken);
        }

        public async Task<Result<PagedResult<ElasticProductReview>>> ListAsync(string productId, PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            if (!await CheckProductExist(productId, cancellationToken))
            {
                return new Result<PagedResult<ElasticProductReview>>(new EntityNotFoundException(typeof(Product), productId));
            }

            var response = await _elasticSearchClient.SearchAsync<ElasticProductReview>(desc => desc
                .Query(qr => qr
                    .Term(x => x.ProductId, productId)
                )
            );

            if (!response.IsValidResponse)
            {
                return new PagedResult<ElasticProductReview>(null,0,0,0);
            }

            return await response.ToPagedResultAsync(queryParams.Skip,queryParams.Length,_elasticSearchClient);

        }

        public async Task<Result<ElasticProductReview>> GetAsync(string productId, string productReviewId, CancellationToken cancellationToken = default)
        {
            if (!await CheckProductExist(productId, cancellationToken))
            {
                return new Result<ElasticProductReview>(new EntityNotFoundException(typeof(Product), productId));
            }

            var response = await _elasticSearchClient.SearchAsync<ElasticProductReview>(desc => desc
                    .Query(qr => qr
                        .Bool(bl => bl
                            .Must(mt => mt
                                .Term(x => x.ProductId, productId)
                                .Term(x => x.Id, productReviewId)
                            )
                        )
                    )
                );

            if (!response.IsValidResponse)
            {
                return new Result<ElasticProductReview>(new EntityNotFoundException(typeof(ElasticProductReview), productReviewId));
            }

            if(response.Documents.Count == 0)
            {
                return new Result<ElasticProductReview>(new EntityNotFoundException(typeof(ElasticProductReview), productReviewId));
            }
       
            return response.Documents.ToArray()[0];
        }
    }
}
