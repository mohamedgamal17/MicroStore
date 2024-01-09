﻿using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Extensions;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Domain.Entities;
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

        public async Task<Result<OrderDto>> GetAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext
                 .Query<OrderStateEntity>()
                 .AsNoTracking()
                 .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.Id == orderId, cancellationToken);


            if (result == null)
            {
                return new Result<OrderDto>(new EntityNotFoundException(typeof(OrderStateEntity), orderId));
            }


            return result;
        }

        public async Task<Result<OrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext
              .Query<OrderStateEntity>()
              .AsNoTracking()
              .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);

            if (result == null)
            {
                return new Result<OrderDto>(new EntityNotFoundException(typeof(OrderStateEntity), orderNumber));
            }

            return result;
        }

        public async Task<Result<PagedResult<OrderDto>>> ListAsync(OrderListQueryModel queryParams , string? userId = null, CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext
                  .Query<OrderStateEntity>()
                  .AsNoTracking()
                  .IgnoreAutoIncludes()
                  .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            query = ApplyQueryFilter(query, queryParams, userId);

            var result = await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

            return result;
        }

        public async Task<Result<PagedResult<OrderDto>>> SearchByOrderNumber(OrderSearchModel model, CancellationToken cancellationToken = default)
        {
            var ordersQuery = _orderDbContext.Query<OrderStateEntity>()
                .AsNoTracking()
                .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider)
                .AsQueryable();


            ordersQuery = from order in ordersQuery
                          where order.OrderNumber == model.OrderNumber
                             || order.OrderNumber.StartsWith(model.OrderNumber)
                             || order.OrderNumber.Contains(model.OrderNumber)
                          select order;


            return await ordersQuery.PageResult(model.Skip, model.Length, cancellationToken);
        }
        private IQueryable<OrderDto> TryToSort(IQueryable<OrderDto> query, string? sortby, bool desc)
        {
            return sortby?.ToLower() switch
            {
                "submission_date" => desc ? query.OrderByDescending(x => x.SubmissionDate)
                    : query.OrderBy(x => x.SubmissionDate),
                _ => query.OrderByDescending(x=> x.Id)
            };
        }


        private IQueryable<OrderDto> ApplyQueryFilter(IQueryable<OrderDto> query, OrderListQueryModel model, string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x=> x.UserId == userId);
            }

            if (!string.IsNullOrEmpty(model.States))
            {
                var states = model.States.Split(',');

                query = query.Where(x => states.Contains(x.CurrentState));
            }

            if (model.StartSubmissionDate != DateTime.MinValue)
            {
                query = query.Where(x => x.SubmissionDate >= model.StartSubmissionDate);
            }

            if (model.EndSubmissionDate != DateTime.MinValue)
            {
                query = query.Where(x => x.SubmissionDate <= model.EndSubmissionDate);
            }

            if(!string.IsNullOrEmpty(model.OrderNumber))
            {
                query = from order in query
                        where order.OrderNumber == model.OrderNumber
                                 || order.OrderNumber.StartsWith(model.OrderNumber)
                                 || order.OrderNumber.Contains(model.OrderNumber)
                              select order;
            }


            return TryToSort(query, model.SortBy, model.Desc); ;
        }
    }
}
