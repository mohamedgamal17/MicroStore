using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace MicroStore.Inventory.Application.Products
{
    [DisableValidation]
    public class ProductQueryService : InventoryApplicationService, IProductQueryService
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public ProductQueryService(IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public async Task<UnitResult<ProductDto>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var query = _inventoryDbContext.Products
                .AsNoTracking()
                .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == id,cancellationToken);

            if (result == null)
                {
                    return UnitResult.Failure<ProductDto>(ErrorInfo.NotFound($"product with product id {id} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<ProductDto>> GetBySkyAsync(string sku, CancellationToken cancellationToken = default)
        {
            var query = _inventoryDbContext.Products
             .AsNoTracking()
             .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Sku == sku,cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<ProductDto>(ErrorInfo.NotFound($"product with product sku {sku} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<PagedResult<ProductDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {

            var query = _inventoryDbContext.Products
             .AsNoTracking()
             .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResult.Success(result);
        }
    }
}
