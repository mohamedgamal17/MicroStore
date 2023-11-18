using Microsoft.Extensions.Options;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Configuration;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Payment.Application.Tests.Fakes
{
    [ExposeServices(typeof(IPaymentMethodProvider), IncludeSelf = true)]
    public class FakePaymentMethod : IPaymentMethodProvider, ITransientDependency
    {
        private readonly PaymentSystem _paymentSystem;

        private readonly IRepository<PaymentRequest> _paymentRequestRepository;

        private readonly IObjectMapper _objectMapper;

        public FakePaymentMethod(IOptions<PaymentSystemOptions> options, IRepository<PaymentRequest> paymentRequestRepository, IObjectMapper objectMapper)
        {
            _paymentSystem = options.Value.Systems.Single(x => x.Name == PaymentMethodConst.ProviderKey);

            _paymentRequestRepository = paymentRequestRepository;
            _objectMapper = objectMapper;
        }


        public Task<Result<PaymentProcessResultDto>> Process(string paymentId, ProcessPaymentRequestModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            var result = new PaymentProcessResultDto
            {
                CheckoutLink = PaymentMethodConst.CheckoutUrl,
                SessionId = paymentId,
                TransactionId = paymentId,
                SuccessUrl = processPaymentModel.ReturnUrl,
                CancelUrl = processPaymentModel.CancelUrl,
                Provider = PaymentMethodConst.ProviderKey
            };

            return Task.FromResult(new Result<PaymentProcessResultDto>(result));
        }



        public async Task<Result<PaymentRequestDto>> Refund(string paymentId, CancellationToken cancellationToken = default)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == paymentId, cancellationToken);

            paymentRequest.MarkAsRefunded(DateTime.UtcNow, "fake");

            await _paymentRequestRepository.UpdateAsync(paymentRequest);

            var result = _objectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);

            return result;

        }

        public async Task<Result<PaymentRequestDto>> Complete(string sessionId, CancellationToken cancellationToken = default)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository.SingleAsync(x => x.Id == sessionId);

            paymentRequest.Complete(PaymentMethodConst.ProviderKey, sessionId, DateTime.UtcNow);

            await _paymentRequestRepository.UpdateAsync(paymentRequest);

            return _objectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);

        }


    }
    [ExposeServices(typeof(IPaymentMethodProvider), IncludeSelf = true)]

    public class FakeNonActivePaymentMethodProvider : IPaymentMethodProvider, ITransientDependency
    {
        public Task<Result<PaymentRequestDto>> Complete(string sessionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PaymentProcessResultDto>> Process(string paymentId, ProcessPaymentRequestModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PaymentRequestDto>> Refund(string paymentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
