using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Extensions;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Entities;
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

        public async Task<Result<ProductDto>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var query = _inventoryDbContext.Products
                .AsNoTracking()
                .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result == null)
            {
                return new Result<ProductDto>(new EntityNotFoundException(typeof(Product), id));
            }

            return result;
        }


        public async Task<Result<PagedResult<ProductDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {

            var query = _inventoryDbContext.Products
             .AsNoTracking()
             .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

            return result;
        }

    }
}
