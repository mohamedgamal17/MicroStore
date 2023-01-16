using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Models;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain;
using System.Net;
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

        public Task<ResponseResult> Process(Guid paymentId, ProcessPaymentModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            var result = new PaymentProcessResultDto
            {
                CheckoutLink = PaymentMethodConst.CheckoutUrl
            };

            return Task.FromResult(Success(HttpStatusCode.Accepted, result));
        }

        public async Task<ResponseResult> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == Guid.Parse(completePaymentModel.Token) , cancellationToken);

            paymentRequest.Complete(PaymentMethodConst.PaymentGatewayName, Guid.NewGuid().ToString(), DateTime.UtcNow);


            await _paymentRequestRepository.UpdateAsync(paymentRequest);


            return Success(HttpStatusCode.Accepted, _objectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest));

        }

        public async Task<ResponseResult> Refund(Guid paymentId, CancellationToken cancellationToken = default)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == paymentId, cancellationToken);
            paymentRequest.MarkAsRefunded(DateTime.UtcNow, "fake");

           return Success(HttpStatusCode.Accepted, await _paymentRequestRepository.UpdateAsync(paymentRequest));
          
        }

        public Task<bool> IsEnabled()
        {
            return Task.FromResult(true);
        }


        private ResponseResult Success<T>(HttpStatusCode statusCode, T result)
            => ResponseResult.Success((int)statusCode, result);

        private ResponseResult Success(HttpStatusCode statusCode)
            => ResponseResult.Success((int)statusCode);

        private ResponseResult Failure(HttpStatusCode statusCode, ErrorInfo error)
            => ResponseResult.Failure((int)statusCode, error);
    }

    [ExposeServices(typeof(IPaymentMethod))]
    public class FakeNotActivePaymentMethod : IPaymentMethod , ITransientDependency
    {
        public string PaymentGatewayName => PaymentMethodConst.NonActiveGateway;

        public Task<ResponseResult> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEnabled()
        {
            return Task.FromResult(false);
        }

        public Task<ResponseResult> Process(Guid paymentId, ProcessPaymentModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult> Refund(Guid paymentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
