using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Common;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Payment.Application.PaymentRequests
{
    public class PaymentRequestQueryService : PaymentApplicationService, IPaymentRequestQueryService
    {
        private readonly IPaymentDbContext _paymentDbContext;

        public PaymentRequestQueryService(IPaymentDbContext paymentDbContext)
        {
            _paymentDbContext = paymentDbContext;
        }
        public async Task<Result<PaymentRequestDto>> GetAsync(string paymentId, CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentRequests
              .AsNoTracking()
              .ProjectTo<PaymentRequestDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == paymentId, cancellationToken);

            if (result == null)
            {
                return new Result<PaymentRequestDto>(new EntityNotFoundException(typeof(PaymentRequest), paymentId));
            }

            return result;
        }

        public async Task<Result<PaymentRequestDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentRequests
              .AsNoTracking()
              .ProjectTo<PaymentRequestDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

            if (result == null)
            {
                return new Result<PaymentRequestDto>(new EntityNotFoundException($"Payment request with order id : {orderId} is not exist"));
            }


            return result;
        }

        public async Task<Result<PaymentRequestDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentRequests
            .AsNoTracking()
            .ProjectTo<PaymentRequestDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);

            if (result == null)
            {
                return new Result<PaymentRequestDto>(new EntityNotFoundException($"Payment request with order number : {orderNumber} is not exist"));
            }


            return result;
        }

        public async Task<Result<PagedResult<PaymentRequestDto>>> ListPaymentAsync(PaymentRequestListQueryModel queryParams, string? userId = null, CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentRequests
              .AsNoTracking()
              .ProjectTo<PaymentRequestDto>(MapperAccessor.Mapper.ConfigurationProvider);


            query = ApplyFilter(query, queryParams, userId);

            var result = await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

            return result;
        }

        public async Task<Result<PagedResult<PaymentRequestDto>>> SearchByOrderNumber(PaymentRequestSearchModel model, CancellationToken cancellationToken = default)
        {
            var paymentRequestsQuery = _paymentDbContext.PaymentRequests
                .AsNoTracking()
                .ProjectTo<PaymentRequestDto>(MapperAccessor.Mapper.ConfigurationProvider)
                .AsQueryable();

            paymentRequestsQuery = from paymentRequest in paymentRequestsQuery
                                   where paymentRequest.OrderNumber == model.OrderNumber
                                         || paymentRequest.OrderNumber.StartsWith(model.OrderNumber)
                                         || paymentRequest.OrderNumber.Contains(model.OrderNumber)
                                   select paymentRequest;


            return await paymentRequestsQuery.PageResult(model.Skip, model.Length, cancellationToken);
        }

        private IQueryable<PaymentRequestDto> TryToSort(IQueryable<PaymentRequestDto> query, string sortBy, bool desc)
        {
            return sortBy.ToLower() switch
            {
                "creation" => desc ? query.OrderByDescending(x => x.CreationTime) : query.OrderBy(x => x.CreationTime),
                "price" => desc ? query.OrderByDescending(x=>x.TotalCost): query.OrderBy(x=> x.TotalCost),
                _ => query
            };
        }


        private IQueryable<PaymentRequestDto> ApplyFilter(IQueryable<PaymentRequestDto> query, PaymentRequestListQueryModel model , string? userId = null)
        {
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrEmpty(model.States))
            {
                var states = model.States.Split(',');

                query = query.Where(x => states.Contains(x.Status));
            }

            if(model.MinPrice != null)
            {
                query = query.Where(x => x.TotalCost >= model.MinPrice);
            }

            if(model.MaxPrice != null)
            {
                query= query.Where(x=>x.TotalCost <=model.MaxPrice);
            }

            if (model.SortBy != null)
            {
                query = TryToSort(query, model.SortBy, model.Desc);
            }

            return query;
        }
    }
}
