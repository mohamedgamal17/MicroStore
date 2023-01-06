using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Abstractions.Common;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Application.Queries
{
    public class GetPaymentSystemListQueryHandler : QueryHandler<GetPaymentSystemListQuery>
    {
        private readonly IPaymentDbContext _paymentDbContext;
        public GetPaymentSystemListQueryHandler(IPaymentDbContext paymentDbContext)
        {
            _paymentDbContext = paymentDbContext;
        }

        public override async Task<ResponseResult> Handle(GetPaymentSystemListQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentSystems
                .AsNoTracking()
                .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.ToListAsync(cancellationToken);

            return Success(HttpStatusCode.OK,  result);
        }
    }
}
