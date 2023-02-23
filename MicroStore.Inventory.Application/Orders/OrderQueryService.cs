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

namespace MicroStore.Inventory.Application.Orders
{
    [DisableValidation]
    public class OrderQueryService : InventoryApplicationService, IOrderQueryService
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public OrderQueryService(IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public async Task<UnitResult<OrderDto>> GetOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var query = _inventoryDbContext.Orders
               .AsNoTracking()
               .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.Id == orderId, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<OrderDto>(ErrorInfo.NotFound( $"order with id : {orderId} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<OrderDto>> GetOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            var query = _inventoryDbContext.Orders
                    .AsNoTracking()
                    .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<OrderDto>(ErrorInfo.NotFound($"order with number : {orderNumber} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<PagedResult<OrderListDto>>> ListOrderAsync(PagingQueryParams queryParams, string? userId = null,  CancellationToken cancellationToken = default)
        {
            var query = _inventoryDbContext.Orders
               .AsNoTracking()
               .ProjectTo<OrderListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if(userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResult.Success(result);
        }
    }


}
