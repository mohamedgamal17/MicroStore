using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Common;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public class PaymentSystemQueryService : PaymentApplicationService, IPaymentSystemQueryService
    {
        private readonly IPaymentDbContext _paymentDbContext;

        public PaymentSystemQueryService(IPaymentDbContext paymentDbContext)
        {
            _paymentDbContext = paymentDbContext;
        }

        public async Task<UnitResult<PaymentSystemDto>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentSystems
                .AsNoTracking()
                .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<PaymentSystemDto>(ErrorInfo.NotFound($"payment sytem with Id : {id} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<PaymentSystemDto>> GetBySystemNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentSystems
               .AsNoTracking()
               .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<PaymentSystemDto>(ErrorInfo.NotFound($"payment sytem with name : {name} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<List<PaymentSystemDto>>> ListPaymentSystemAsync(CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentSystems
              .AsNoTracking()
              .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.ToListAsync(cancellationToken);

            return UnitResult.Success(result);
        }
    }
}
