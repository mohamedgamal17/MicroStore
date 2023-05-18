using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Products;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;
namespace MicroStore.Catalog.Application.Products
{
    [DisableValidation]
    public class ProductQueryService : CatalogApplicationService, IProductQueryService
    {
        private readonly ICatalogDbContext _catalogDbContext;
        public ProductQueryService(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public async Task<Result<ProductDto>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.Products
                 .AsNoTracking()
                 .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var product = await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (product == null)
            {
                return new Result<ProductDto>(new EntityNotFoundException(typeof(Product), id));

            }

            return product;
        }

        public async Task<Result<PagedResult<ProductDto>>> ListAsync(PagingAndSortingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.Products.AsQueryable()
                         .AsNoTracking()
                         .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (queryParams.SortBy != null)
            {
                query = TryToSort(query, queryParams.SortBy, queryParams.Desc);
            }

            var pagingResult = await query.PageResult(queryParams.Skip, queryParams.Lenght, cancellationToken);

            return pagingResult;
        }

        public async Task<Result<List<ProductImageDto>>> ListProductImagesAsync(string productid, CancellationToken cancellationToken = default)
        {
            var isProductExist = await _catalogDbContext.Products.AnyAsync(x => x.Id == productid, cancellationToken);

            if (!isProductExist)
            {
                return new Result<List<ProductImageDto>>(new EntityNotFoundException(typeof(Product), productid));
            }


            var result = await _catalogDbContext.Products.Where(x => x.Id == productid).SelectMany(x => x.ProductImages)
                .ProjectTo<ProductImageDto>(MapperAccessor.Mapper.ConfigurationProvider)
                .ToListAsync();

            return result;
        }

        public async Task<Result<PagedResult<ProductDto>>> SearchAsync(ProductSearchModel model, CancellationToken cancellationToken = default)
        {
            var productsQuery = _catalogDbContext.Products
                    .AsNoTracking()
                    .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider)
                    .AsQueryable();

            productsQuery = from product in productsQuery
                            where product.Name.Contains(model.KeyWords) ||
                                product.ShortDescription.Contains(model.KeyWords) ||
                                product.LongDescription.Contains(model.KeyWords) ||
                                product.Sku.Contains(model.KeyWords)
                            select product;

            if(model.CategoriesIds is not null)
            {
                productsQuery = from product in productsQuery
                                from categoryId in model.CategoriesIds
                                where product.ProductCategories.Any(c => c.CategoryId == categoryId)
                                select product;
            }

            if(model.ManufactureriesIds is not null)
            {
                productsQuery = from product in productsQuery
                                from manufacturerId in model.ManufactureriesIds
                                where product.ProductManufacturers.Any(c => c.ManufacturerId == manufacturerId)
                                select product;
            }


            return await productsQuery.PageResult(model.Skip, model.Lenght , cancellationToken);
        }

        public IQueryable<ProductDto> TryToSort(IQueryable<ProductDto> query, string sortBy, bool desc = false)
        {
            return sortBy.ToLower() switch
            {
                "name" => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "price" => desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                "old_price" => desc ? query.OrderByDescending(x => x.OldPrice) : query.OrderBy(x => x.OldPrice),
                _ => query
            };
        }
    }


}
