﻿using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Categories
{
    public class CategoryQueryService : CatalogApplicationService, ICategoryQueryService
    {
        private readonly ICatalogDbContext _catalogDbContext;

        public CategoryQueryService(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public async Task<UnitResult<CategoryDto>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            Category? category = await _catalogDbContext.Categories
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (category == null)
            {
                return UnitResult.Failure<CategoryDto>(ErrorInfo.NotFound($"Category with id : {id} is not exist"));
         
            }

            return UnitResult.Success(ObjectMapper.Map<Category, CategoryDto>(category));
        }

        public async Task<UnitResult<List<CategoryDto>>> ListAsync(SortingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.Categories
                .AsNoTracking()
                .ProjectTo<CategoryDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (queryParams.SortBy != null)
            {
                query = TryToSort(query, queryParams.SortBy, queryParams.Desc);
            }

            var result = await query.ToListAsync(cancellationToken);

            return UnitResult.Success(result);
        }

        private IQueryable<CategoryDto> TryToSort(IQueryable<CategoryDto> query, string sortBy, bool desc)
        {
            return sortBy.ToLower() switch
            {
                "name" => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                _ => query
            };
        }
    }
}
