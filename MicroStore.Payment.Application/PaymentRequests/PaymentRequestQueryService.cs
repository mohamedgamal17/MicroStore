using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Extensions;
using MicroStore.BuildingBlocks.Utils.Results;
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
              .AsNoTracking();
              

            query = ApplyFilter(query, queryParams, userId);

            var queryResult = await query
                .PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

            var items = ObjectMapper.Map<IEnumerable<PaymentRequest>, IEnumerable<PaymentRequestDto>>(queryResult.Items);

            var pagedResult = new PagedResult<PaymentRequestDto>(items, queryResult.TotalCount, queryResult.Skip, queryResult.Lenght);

            return pagedResult;
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


            return await paymentRequestsQuery
                .PageResult(model.Skip, model.Length, cancellationToken);
        }

        private IQueryable<PaymentRequest> TryToSort(IQueryable<PaymentRequest> query, string sortBy, bool desc)
        {
            return sortBy.ToLower() switch
            {
                "creation" => desc ? query.OrderByDescending(x => x.CreationTime) : query.OrderBy(x => x.CreationTime),
                "price" => desc ? query.OrderByDescending(x=>x.TotalCost): query.OrderBy(x=> x.TotalCost),
                _ => query.OrderByDescending(x=> x.CreationTime)
            };
        }


        private IQueryable<PaymentRequest> ApplyFilter(IQueryable<PaymentRequest> query, PaymentRequestListQueryModel model , string? userId = null)
        {
            if (!userId.IsNullOrEmpty())
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrEmpty(model.OrderNumber))
            {
                var orderNumber = model.OrderNumber.ToLower();

                query = from paymentRequest in query
                        where paymentRequest.OrderNumber == orderNumber
                              || paymentRequest.OrderNumber.ToLower().StartsWith(orderNumber)
                              || paymentRequest.OrderNumber.ToLower().Contains(orderNumber)
                        select paymentRequest;
            }

            if (!model.Status.IsNullOrEmpty())
            {
                var status = Enum.Parse<PaymentStatus>(model.Status);

                query = query.Where(x => x.State == status);
            }

            if(model.MinPrice  > -1)
            {
                query = query.Where(x => x.TotalCost >= model.MinPrice);
            }

            if(model.MaxPrice > -1)
            {
                query= query.Where(x=>x.TotalCost <=model.MaxPrice);
            }

            if (model.StartDate != DateTime.MinValue)
            {
                var startDate = model.StartDate;

                query = query.Where(x => x.CreationTime >= startDate);
            }

            if(model.EndDate != DateTime.MinValue)
            {
                var endDate = model.EndDate;
                query = query.Where(x => x.CreationTime <= endDate);
            }

            if(!model.SortBy.IsNullOrEmpty())
            {
                query = TryToSort(query, model.SortBy!, model.Desc);

            }
            else
            {
                query = query.OrderByDescending(x => x.CreationTime);
            }

            return query;
        }

        public async Task<Result<List<PaymentRequestDto>>> ListPaymentByOrderIdsAsync(List<string> orderIds, CancellationToken cancellationToken = default)
        {
            if(orderIds != null && orderIds.Count  > 0)
            {
                var query = _paymentDbContext.PaymentRequests.AsNoTracking();

                var payments = await query.Where(x => orderIds.Contains(x.OrderId)).ToListAsync(cancellationToken);

                return ObjectMapper.Map<List<PaymentRequest>, List<PaymentRequestDto>>(payments);
            }

            return new List<PaymentRequestDto>();
        }

        public async Task<Result<List<PaymentRequestDto>>> ListPaymentByOrderNumbersAsync(List<string> orderNumbers, CancellationToken cancellationToken = default)
        {
            if (orderNumbers != null && orderNumbers.Count > 0)
            {
                var query = _paymentDbContext.PaymentRequests.AsNoTracking();

                var payments = await query.Where(x => orderNumbers.Contains(x.OrderNumber)).ToListAsync(cancellationToken);

                return ObjectMapper.Map<List<PaymentRequest>, List<PaymentRequestDto>>(payments);
            }

            return new List<PaymentRequestDto>();
        }
    }
}
