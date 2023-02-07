using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.StateMachines;
using System.Net;

namespace MicroStore.Ordering.Application.Orders
{
    public class OrderQueryHandler : RequestHandler,
        IQueryHandler<GetOrderListQuery, PagedResult<OrderListDto>>,
        IQueryHandler<GetUserOrderListQuery,PagedResult<OrderListDto>>,
        IQueryHandler<GetOrderQuery,OrderDto>

    {

        private readonly IOrderDbContext _orderDbContext;

        public OrderQueryHandler(IOrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async  Task<ResponseResult<PagedResult<OrderListDto>>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = _orderDbContext
               .Query<OrderStateEntity>()
               .AsNoTracking()
               .ProjectTo<OrderListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (request.SortBy != null)
            {
                query = TryToSort(query, request.SortBy, request.Desc);
            }

            var result = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<PagedResult<OrderListDto>>> Handle(GetUserOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = _orderDbContext
                .Query<OrderStateEntity>()
                .AsNoTracking()
                .ProjectTo<OrderListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (request.SortBy != null)
            {
                query = TryToSort(query, request.SortBy, request.Desc);
            }

            var result = await query.Where(x => x.UserId == request.UserId)
                  .PageResult(request.PageNumber, request.PageSize, cancellationToken);


            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var query = _orderDbContext
                .Query<OrderStateEntity>()
                .AsNoTracking()
                .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            if (result == null)
            {
                return Failure<OrderDto>(HttpStatusCode.NotFound, $"order with id :{request.OrderId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }

        private IQueryable<OrderListDto> TryToSort(IQueryable<OrderListDto> query, string sortby, bool desc)
        {
            return sortby.ToLower() switch
            {
                "submission_date" => desc ? query.OrderByDescending(x => x.SubmissionDate)
                    : query.OrderBy(x => x.SubmissionDate),
                _ => query
            };
        }
    }
}
