using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Common;
using MicroStore.Payment.Domain.Shared.Dtos;
using System.Net;

namespace MicroStore.Payment.Application.PaymentRequests
{
    public class PaymentRequestQueryHandler : RequestHandler,
        IQueryHandler<GetPaymentRequestListQuery, PagedResult<PaymentRequestListDto>>,
        IQueryHandler<GetUserPaymentRequestListQuery,PagedResult<PaymentRequestListDto>>,
        IQueryHandler<GetPaymentRequestQuery, PaymentRequestDto>,
        IQueryHandler<GetPaymentRequestWithOrderIdQuery, PaymentRequestDto>
    {
        private readonly IPaymentDbContext _paymentDbContext;

        public PaymentRequestQueryHandler(IPaymentDbContext paymentDbContext)
        {
            _paymentDbContext = paymentDbContext;
        }

        public async Task<ResponseResult<PagedResult<PaymentRequestListDto>>> Handle(GetPaymentRequestListQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentRequests
                .AsNoTracking()
                .ProjectTo<PaymentRequestListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (request.SortBy != null)
            {
                query = TryToSort(query, request.SortBy, request.Desc);
            }

            var result = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<PagedResult<PaymentRequestListDto>>> Handle(GetUserPaymentRequestListQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentRequests
               .AsNoTracking()
               .ProjectTo<PaymentRequestListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (request.SortBy != null)
            {
                query = TryToSort(query, request.SortBy, request.Desc);
            }

            var result = await query.Where(x => x.CustomerId == request.UserId)
                .PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<PaymentRequestDto>> Handle(GetPaymentRequestQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentRequests
              .AsNoTracking()
              .ProjectTo<PaymentRequestDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.PaymentRequestId, cancellationToken);

            if (result == null)
            {
                var error = new ErrorInfo
                {
                    Message = $"Payment request with id : {request.PaymentRequestId} is not exist"
                };

                return Failure<PaymentRequestDto>(HttpStatusCode.NotFound, error);
            }


            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<PaymentRequestDto>> Handle(GetPaymentRequestWithOrderIdQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentRequests
                .AsNoTracking()
                .ProjectTo<PaymentRequestDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.OrderId == request.OrderId, cancellationToken);

            if (result == null)
            {
                var error = new ErrorInfo
                {
                    Message = $"Payment request with order id : {request.OrderId} is not exist"
                };

                return Failure<PaymentRequestDto>(HttpStatusCode.NotFound, error);
            }


            return Success(HttpStatusCode.OK, result);
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
