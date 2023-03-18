﻿using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Products
{
    public interface IProductQueryService : IApplicationService
    {
        Task<ResultV2<PagedResult<ProductDto>>> ListAsync(PagingQueryParams queryParams , CancellationToken cancellationToken = default);

        Task<ResultV2<ProductDto>> GetAsync(string id , CancellationToken cancellationToken = default);

        Task<ResultV2<ProductDto>> GetBySkyAsync(string sku, CancellationToken cancellationToken = default);
    }
}
