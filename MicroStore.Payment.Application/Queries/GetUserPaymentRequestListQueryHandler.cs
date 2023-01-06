using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Abstractions.Common;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;
namespace MicroStore.Payment.Application.Queries
{
    public class GetUserPaymentRequestListQueryHandler : QueryHandler<GetUserPaymentRequestListQuery>
    {
        private readonly IPaymentDbContext _paymentDbContext;

        public GetUserPaymentRequestListQueryHandler( IPaymentDbContext paymentDbContext)
        {
            _paymentDbContext = paymentDbContext;
        }

        public override async Task<ResponseResult> Handle(GetUserPaymentRequestListQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentRequests
                .AsNoTracking()
                .ProjectTo<PaymentRequestListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if(request.SortBy != null)
            {
                query = TryToSort(query, request.SortBy, request.Desc);
            }

            var result = await query.Where(x => x.CustomerId == request.UserId)
                .PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        private IQueryable<PaymentRequestListDto> TryToSort(IQueryable<PaymentRequestListDto> query, string sortBy, bool desc)
        {
            return sortBy.ToLower() switch
            {
                "creation" => desc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                _ => query
            };
        }
    }
}
