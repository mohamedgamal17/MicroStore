using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Abstractions.Common;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Application.Abstractions.Queries;
using MicroStore.Ordering.Application.Abstractions.StateMachines;
using System.Net;

namespace MicroStore.Ordering.Application.Queries
{
    public class GetOrderListQueryHandler : QueryHandler<GetOrderListQuery,PagedResult<OrderListDto>>
    {

        private readonly IOrderDbContext _orderDbContext;

        public GetOrderListQueryHandler( IOrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        public override async Task<ResponseResult<PagedResult<OrderListDto>>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = _orderDbContext
                .Query<OrderStateEntity>()
                .AsNoTracking()
                .ProjectTo<OrderListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if(request.SortBy != null)
            {
                query = TryToSort(query,request.SortBy,request.Desc);
            }

            var result = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }


        private IQueryable<OrderListDto> TryToSort(IQueryable<OrderListDto> query,string sortby , bool desc)
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
