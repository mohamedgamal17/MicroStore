using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Abstractions.Common;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Payment.Application.Queries
{
    public class GetPaymentSystemWithNameQueryHandler : QueryHandler<GetPaymentSystemWithNameQuery>
    {
        private readonly IPaymentDbContext _paymentDbContext;

        public GetPaymentSystemWithNameQueryHandler(IPaymentDbContext paymentDbContext)
        {
            _paymentDbContext = paymentDbContext;
        }

        public override async Task<ResponseResult> Handle(GetPaymentSystemWithNameQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentSystems
                .AsNoTracking()
                .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Name == request.SystemName, cancellationToken);

            if (result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"payment sytem with name : {request.SystemName} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
