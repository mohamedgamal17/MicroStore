﻿using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;

namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryQueryService
    {
        Task<Result<List<CategoryDto>>> ListAsync(SortingQueryParams queryParams,CancellationToken cancellationToken = default);
        Task<Result<CategoryDto>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
