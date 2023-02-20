using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.StateMachines;
using Volo.Abp.Validation;

namespace MicroStore.Ordering.Application.Orders
{
    [DisableValidation]
    public class OrderQueryService : OrderApplicationService, IOrderQueryService
    {
        private readonly IOrderDbContext _orderDbContext;

        public OrderQueryService(IOrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<UnitResultV2<OrderDto>> GetAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext
                 .Query<OrderStateEntity>()
                 .AsNoTracking()
                 .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.Id == orderId, cancellationToken);

            if (result == null)
            {
                return UnitResultV2.Failure<OrderDto>(ErrorInfo.NotFound($"order with id :{orderId} is not exist"));
            }

            return UnitResultV2.Success(result);
        }

        public async Task<UnitResultV2<OrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default )
        {
            var query = _orderDbContext
              .Query<OrderStateEntity>()
              .AsNoTracking()
              .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);

            if (result == null)
            {
                return UnitResultV2.Failure<OrderDto>(ErrorInfo.NotFound($"order with order number :{orderNumber} is not exist"));
            }

            return UnitResultV2.Success(result);
        }

        public async Task<UnitResultV2<PagedResult<OrderListDto>>> ListAsync(PagingAndSortingQueryParams queryParams , string? userId = null , CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext
                  .Query<OrderStateEntity>()
                  .AsNoTracking()
                  .ProjectTo<OrderListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if(userId != null)
            {
                query = query.Where(x=> x.UserId== userId);
            }

            if (queryParams.SortBy != null)
            {
                query = TryToSort(query, queryParams.SortBy, queryParams.Desc);
            }

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResultV2.Success(result);
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
