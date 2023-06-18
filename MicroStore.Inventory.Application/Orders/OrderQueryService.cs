using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Domain.OrderAggregate;
using Volo.Abp.Domain.Entities;
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

        public async Task<Result<OrderDto>> GetOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var query = _inventoryDbContext.Orders
               .AsNoTracking()
               .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.Id == orderId, cancellationToken);

            if (result == null)
            {
                return new Result<OrderDto>(new EntityNotFoundException(typeof(Order), orderId));
            }

            return result;
        }

        public async Task<Result<OrderDto>> GetOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            var query = _inventoryDbContext.Orders
                    .AsNoTracking()
                    .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);

            if (result == null)
            {
                return new Result<OrderDto>(new EntityNotFoundException($"order with number : {orderNumber} is not exist"));
            }

            return result;
        }

        public async Task<Result<PagedResult<OrderListDto>>> ListOrderAsync(PagingQueryParams queryParams, string? userId = null,  CancellationToken cancellationToken = default)
        {
            var query = _inventoryDbContext.Orders
               .AsNoTracking()
               .ProjectTo<OrderListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if(userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            var result = await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

            return result;
        }
    }


}
