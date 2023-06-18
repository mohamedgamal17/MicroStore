using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.ProductReviews;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.ProductReviews
{
    public class ProductReviewService : CatalogApplicationService, IProductReviewService
    {
        private readonly IRepository<ProductReview> _productReviewRepository;

        private readonly IRepository<Product> _productRepository;

        private readonly ICatalogDbContext _catalogDbContext;
        public ProductReviewService(IRepository<ProductReview> productReviewRepository, IRepository<Product> productRepository, ICatalogDbContext catalogDbContext)
        {
            _productReviewRepository = productReviewRepository;
            _productRepository = productRepository;
            _catalogDbContext = catalogDbContext;
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

        public async Task<Result<PagedResult<ProductReviewDto>>> ListAsync(string productId, PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            if (!await CheckProductExist(productId, cancellationToken))
            {
                return new Result<PagedResult<ProductReviewDto>>(new EntityNotFoundException(typeof(Product), productId));
            }

            var query = _catalogDbContext.ProductReviews
                .AsNoTracking()
                .ProjectTo<ProductReviewDto>(MapperAccessor.Mapper.ConfigurationProvider)
                .AsQueryable();

            var productReviews = await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);


            return productReviews;

        }

        public async Task<Result<ProductReviewDto>> GetAsync(string productId, string productReviewId, CancellationToken cancellationToken = default)
        {
            if (!await CheckProductExist(productId, cancellationToken))
            {
                return new Result<ProductReviewDto>(new EntityNotFoundException(typeof(Product), productId));
            }

            var query = _catalogDbContext.ProductReviews
               .AsNoTracking()
               .ProjectTo<ProductReviewDto>(MapperAccessor.Mapper.ConfigurationProvider)
               .AsQueryable();

            var productReview = await query.SingleOrDefaultAsync(x => x.Id == productReviewId && x.ProductId == productId);

            if (productReview == null)
            {
                return new Result<ProductReviewDto>(new EntityNotFoundException(typeof(ProductReview), productReviewId));
            }

            return productReview;
        }
    }
}
