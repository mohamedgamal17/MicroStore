using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Application.Dtos;
using System.Net;

namespace MicroStore.Inventory.Application.Orders
{
    public class OrderQueryHandler : RequestHandler,
        IQueryHandler<GetOrderListQuery, PagedResult<OrderListDto>>,
        IQueryHandler<GetUserOrderListQuery, PagedResult<OrderListDto>>,
        IQueryHandler<GetOrderWithExternalIdQuery,OrderDto>,
        IQueryHandler<GetOrderQuery,OrderDto>
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public OrderQueryHandler(IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public async Task<ResponseResult<PagedResult<OrderListDto>>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Orders
                .AsNoTracking()
                .ProjectTo<OrderListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }
        public async Task<ResponseResult<PagedResult<OrderListDto>>> Handle(GetUserOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Orders
                  .AsNoTracking()
                  .ProjectTo<OrderListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.Where(x => x.UserId == request.UserId)
                .PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<OrderDto>> Handle(GetOrderWithExternalIdQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Orders
              .AsNoTracking()
              .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.ExternalOrderId == request.ExternalOrderId);

            if (result == null)
            {
                return Failure<OrderDto>(HttpStatusCode.NotFound, $"order with external id {request.ExternalOrderId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Orders
             .AsNoTracking()
             .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.Id == request.OrderId);

            if (result == null)
            {
                return Failure<OrderDto>(HttpStatusCode.NotFound, $"order with id {request.OrderId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }

      
    }
}
