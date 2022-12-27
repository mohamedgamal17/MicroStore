using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Models;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain;
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

        public Task<PaymentProcessResultDto> Process(Guid paymentId, ProcessPaymentModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new PaymentProcessResultDto
            {
                CheckoutLink = PaymentMethodConst.CheckoutUrl
            });
        }

        public async Task<PaymentRequestCompletedDto> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == Guid.Parse(completePaymentModel.Token) , cancellationToken);

            paymentRequest.Complete(PaymentMethodConst.PaymentGatewayName, Guid.NewGuid().ToString(), DateTime.UtcNow);


            await _paymentRequestRepository.UpdateAsync(paymentRequest);


            return _objectMapper.Map<PaymentRequest, PaymentRequestCompletedDto>(paymentRequest);

        }

        public async Task Refund(Guid paymentId, CancellationToken cancellationToken = default)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == paymentId, cancellationToken);
            paymentRequest.MarkAsRefunded(DateTime.UtcNow, "fake");

            await _paymentRequestRepository.UpdateAsync(paymentRequest);
          
        }

        public Task<bool> IsEnabled()
        {
            return Task.FromResult(true);
        }
    }

    [ExposeServices(typeof(IPaymentMethod))]
    public class FakeNotActivePaymentMethod : IPaymentMethod , ITransientDependency
    {
        public string PaymentGatewayName => PaymentMethodConst.NonActiveGateway;

        public Task<PaymentRequestCompletedDto> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEnabled()
        {
            return Task.FromResult(false);
        }

        public Task<PaymentProcessResultDto> Process(Guid paymentId, ProcessPaymentModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Refund(Guid paymentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
