using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
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

        public async Task<Result<PagedResult<ProductDto>>> ListAsync(ProductListQueryModel queryParams, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.Products.AsQueryable()
                         .AsNoTracking()
                         .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            query = ApplyQueryFilteration(query, queryParams);

            var pagingResult = await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

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
                                product.Sku.Contains(model.KeyWords) ||
                                product.ProductCategories.Any(x=> x.Category.Name.Contains(model.KeyWords)) ||
                                product.ProductManufacturers.Any(x=> x.Manufacturer.Name.Contains(model.KeyWords))
                            select product;

            return await productsQuery.PageResult(model.Skip, model.Length , cancellationToken);
        }

        

        private IQueryable<ProductDto> ApplyQueryFilteration(IQueryable<ProductDto> query , ProductListQueryModel model)
        {
            if (!string.IsNullOrEmpty(model.Categories))
            {
                var categories = model.Categories.Split(',');

                query = query.Where(x => x.ProductCategories.Any(x => categories.Contains(x.Category.Name)));
            }

            if (!string.IsNullOrEmpty(model.Manufacturers))
            {
                var manufacturers = model.Manufacturers.Split(',');

                query = query.Where(x => x.ProductManufacturers.Any(x => manufacturers.Contains(x.Manufacturer.Name)));
            }

            if (!string.IsNullOrEmpty(model.Tags))
            {
                var tags = model.Tags.Split(",");
                query = query.Where(x => x.ProductTags.Any(x => tags.Contains(x.Name)));
            }


            if(model.MinPrice != null)
            {
                query = query.Where(x => x.Price >= model.MinPrice);
            }

            if(model.MaxPrice != null)
            {
                query = query.Where(x => x.Price <= model.MaxPrice);
            }

            if(model.SortBy != null)
            {
                query = TryToSort(query, model.SortBy, model.Desc);
            }

            return query;

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
