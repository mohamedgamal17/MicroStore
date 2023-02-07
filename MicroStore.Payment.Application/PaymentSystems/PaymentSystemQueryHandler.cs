using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Common;
using MicroStore.Payment.Domain.Shared.Dtos;
using System.Net;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public class PaymentSystemQueryHandler : RequestHandler,
        IQueryHandler<GetPaymentSystemListQuery, ListResultDto<PaymentSystemDto>>,
        IQueryHandler<GetPaymentSystemQuery , PaymentSystemDto>,
        IQueryHandler<GetPaymentSystemWithNameQuery, PaymentSystemDto>
    {
        private readonly IPaymentDbContext _paymentDbContext;

        public PaymentSystemQueryHandler(IPaymentDbContext paymentDbContext)
        {
            _paymentDbContext = paymentDbContext;
        }

        public async Task<ResponseResult<ListResultDto<PaymentSystemDto>>> Handle(GetPaymentSystemListQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentSystems
               .AsNoTracking()
               .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.ToListAsync(cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<PaymentSystemDto>> Handle(GetPaymentSystemQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentSystems
                .AsNoTracking()
                .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.SystemId, cancellationToken);

            if (result == null)
            {
                return Failure<PaymentSystemDto>(HttpStatusCode.NotFound, $"payment sytem with Id : {request.SystemId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<PaymentSystemDto>> Handle(GetPaymentSystemWithNameQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentSystems
                .AsNoTracking()
                .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Name == request.SystemName, cancellationToken);

            if (result == null)
            {
                return Failure<PaymentSystemDto>(HttpStatusCode.NotFound, $"payment sytem with name : {request.SystemName} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
