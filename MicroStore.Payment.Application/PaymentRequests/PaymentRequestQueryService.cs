using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Common;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;
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

            return  result;
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

        public async Task<Result<PagedResult<PaymentRequestListDto>>> ListPaymentAsync(PagingAndSortingQueryParams queryParams, string? userId = null, CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentRequests
              .AsNoTracking()
              .ProjectTo<PaymentRequestListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (queryParams.SortBy != null)
            {
                query = TryToSort(query, queryParams.SortBy, queryParams.Desc);
            }

            var result = await query.PageResult(queryParams.Skip, queryParams.Lenght, cancellationToken);

            return result;
        }

        private IQueryable<PaymentRequestListDto> TryToSort(IQueryable<PaymentRequestListDto> query, string sortBy, bool desc)
        {
            return sortBy.ToLower() switch
            {
                "creation" => desc ? query.OrderByDescending(x => x.CreationTime) : query.OrderBy(x => x.CreationTime),
                _ => query
            };
        }
    }
}
