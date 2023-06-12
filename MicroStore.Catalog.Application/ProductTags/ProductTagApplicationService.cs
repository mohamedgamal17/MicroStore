using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.ProductTags;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.ProductTags
{
    public class ProductTagApplicationService : CatalogApplicationService , IProductTagApplicationService
    {
        private readonly IRepository<ProductTag> _productTagRepository;

        private readonly ICatalogDbContext _catalogDbContext;
        public ProductTagApplicationService(IRepository<ProductTag> productTagRepository, ICatalogDbContext catalogDbContext)
        {
            _productTagRepository = productTagRepository;
            _catalogDbContext = catalogDbContext;
        }

        public async Task<Result<ProductTagDto>> CreateAsync(ProductTagModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateProductTag(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ProductTagDto>(validationResult.Exception);
            }

            var productTag = new ProductTag
            {
                Name = model.Name,
                Description = model.Description,
            };

            await _productTagRepository.InsertAsync(productTag,cancellationToken: cancellationToken);

            return ObjectMapper.Map<ProductTag, ProductTagDto>(productTag);

        }

        public async Task<Result<ProductTagDto>> GetAsync(string productTagId, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.ProductTags
              .AsNoTracking()
              .ProjectTo<ProductTagDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var productTag = await query.SingleOrDefaultAsync(x => x.Id == productTagId, cancellationToken);

            if (productTag == null)
            {
                return new Result<ProductTagDto>(new EntityNotFoundException(typeof(ProductTag), productTagId));
            }


            return productTag;

        }

        public async Task<Result<List<ProductTagDto>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.ProductTags
                .AsNoTracking()
                .ProjectTo<ProductTagDto>(MapperAccessor.Mapper.ConfigurationProvider);

            return await query.ToListAsync(cancellationToken);

        }

        public async Task<Result<Unit>> RemoveAsync(string productTagId, CancellationToken cancellationToken = default)
        {
            var productTag = await _productTagRepository.SingleOrDefaultAsync(x => x.Id == productTagId, cancellationToken);

            if(productTag == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(ProductTag), productTagId));
            }

            await _productTagRepository.DeleteAsync(productTag, cancellationToken: cancellationToken);

            return Unit.Value;
        }

        public async Task<Result<ProductTagDto>> UpdateAsync(string productTagId, ProductTagModel model, CancellationToken cancellationToken = default)
        {
            var productTag = await _productTagRepository.SingleOrDefaultAsync(x => x.Id == productTagId, cancellationToken);

            if (productTag == null)
            {
                return new Result<ProductTagDto>(new EntityNotFoundException(typeof(ProductTag), productTagId));
            }

            var validationResult = await ValidateProductTag(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ProductTagDto>(validationResult.Exception);
            }

            productTag.Name = model.Name;

            productTag.Description = model.Description;

            await _productTagRepository.UpdateAsync(productTag, cancellationToken: cancellationToken);

            return ObjectMapper.Map<ProductTag, ProductTagDto>(productTag);
        }

        public async Task<Result<Unit>> ValidateProductTag(ProductTagModel model , string? productTagId = null, CancellationToken cancellationToken= default)
        {
            var query = await _productTagRepository.GetQueryableAsync();

            if(productTagId != null)
            {
                query = query.Where(x => x.Id == productTagId);
            }

            bool isProductTagNameExist = await query.AnyAsync(x => x.Name == model.Name, cancellationToken);

            if (isProductTagNameExist)
            {
                return new Result<Unit>(new UserFriendlyException($"Product tag name : {model.Name} is already exist"));
            }

            return Unit.Value;
        }

    }
}
