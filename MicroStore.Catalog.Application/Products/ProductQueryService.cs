﻿using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
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

        public async Task<UnitResultV2<ProductDto>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.Products
                 .AsNoTracking()
                 .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var product = await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (product == null)
            {
                return UnitResultV2.Failure<ProductDto>(ErrorInfo.NotFound($"Product entity with id : {id} is not found"));
            }

            return UnitResultV2.Success(product);
        }

        public async Task<UnitResultV2<PagedResult<ProductListDto>>> ListAsync(PagingAndSortingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.Products.AsQueryable()
                         .AsNoTracking()
                         .ProjectTo<ProductListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (queryParams.SortBy != null)
            {
                query = TryToSort(query, queryParams.SortBy, queryParams.Desc);
            }

            var pagingResult = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResultV2.Success(pagingResult);
        }

        public IQueryable<ProductListDto> TryToSort(IQueryable<ProductListDto> query, string sortBy, bool desc = false)
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