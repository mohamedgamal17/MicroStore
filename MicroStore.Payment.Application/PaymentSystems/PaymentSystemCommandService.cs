using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public class PaymentSystemCommandService : PaymentApplicationService , IPaymentSystemCommandService
    {
        private readonly IRepository<PaymentSystem> _paymentSystemRepository;

        public PaymentSystemCommandService(IRepository<PaymentSystem> paymentSystemRepository)
        {
            _paymentSystemRepository = paymentSystemRepository;
        }

        public async Task<UnitResult<PaymentSystemDto>> EnablePaymentSystemAsync(string systemName, bool isEnabled, CancellationToken cancellationToken = default)
        {
            PaymentSystem? paymentSystem = await _paymentSystemRepository.SingleOrDefaultAsync(x => x.Name == systemName,cancellationToken);

            if (paymentSystem == null)
            {
                return UnitResult.Failure<PaymentSystemDto>(ErrorInfo.NotFound( $"Payment system with name : {systemName}, is not exist" ));
            }

            paymentSystem.IsEnabled = isEnabled;

            await _paymentSystemRepository.UpdateAsync(paymentSystem, cancellationToken: cancellationToken);



            return UnitResult.Success(ObjectMapper.Map<PaymentSystem, PaymentSystemDto>(paymentSystem));
        }
    }
}
