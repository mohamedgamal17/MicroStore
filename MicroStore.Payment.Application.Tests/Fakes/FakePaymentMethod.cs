using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Payment.Application.Tests.Fakes
{
    [ExposeServices(typeof(IPaymentMethod))]
    public class FakePaymentMethod : IPaymentMethod, ITransientDependency
    {
        public string PaymentGatewayName => PaymentMethodConst.PaymentGatewayName;

        private readonly IRepository<PaymentRequest> _paymentRequestRepository;

        private readonly IObjectMapper _objectMapper;

        public FakePaymentMethod(IRepository<PaymentRequest> paymentRequestRepository, IObjectMapper objectMapper)
        {
            _paymentRequestRepository = paymentRequestRepository;
            _objectMapper = objectMapper;
        }

        public Task<ResultV2<PaymentProcessResultDto>> Process(string paymentId, ProcessPaymentRequestModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            var result = new PaymentProcessResultDto
            {
                CheckoutLink = PaymentMethodConst.CheckoutUrl
            };

            return Task.FromResult(new ResultV2<PaymentProcessResultDto>( result));
        }

    

        public async Task<ResultV2<PaymentRequestDto>> Refund(string paymentId, CancellationToken cancellationToken = default)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == paymentId, cancellationToken);

            paymentRequest.MarkAsRefunded(DateTime.UtcNow, "fake");

            await _paymentRequestRepository.UpdateAsync(paymentRequest);

            var result = _objectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);

            return result;
          
        }

        private UnitResult<T> Success<T>( T result)
            => UnitResult.Success(result);

    }

    [ExposeServices(typeof(IPaymentMethod))]
    public class FakeNotActivePaymentMethod : IPaymentMethod , ITransientDependency
    {
        public string PaymentGatewayName => PaymentMethodConst.NonActiveGateway;

        public Task<ResultV2<PaymentProcessResultDto>> Process(string paymentId, ProcessPaymentRequestModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResultV2<PaymentRequestDto>> Refund(string paymentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

}
