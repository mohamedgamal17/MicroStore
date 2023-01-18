using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Models;
using MicroStore.Payment.Application.Extensions;
using MicroStore.Payment.Domain;
using System.Net;
namespace MicroStore.Payment.Application.Commands
{
    public class ProcessPaymentRequestCommandHandler : CommandHandler<ProcessPaymentRequestCommand,PaymentProcessResultDto>
    {
        private readonly IPaymentMethodResolver _paymentMethodResolver;

        private readonly IPaymentRequestRepository _paymentRequestRepository;

        public ProcessPaymentRequestCommandHandler(IPaymentMethodResolver paymentMethodResolver, IPaymentRequestRepository paymentRequestRepository)
        {
            _paymentMethodResolver = paymentMethodResolver;
            _paymentRequestRepository = paymentRequestRepository;
        }

        public override  async Task<ResponseResult<PaymentProcessResultDto>> Handle(ProcessPaymentRequestCommand request, CancellationToken cancellationToken)
        {
            var paymentRequest = await _paymentRequestRepository.GetPaymentRequest(request.PaymentId, cancellationToken);

            if(paymentRequest == null)
            {
                return Failure(HttpStatusCode.NotFound,new ErrorInfo { Message = $"Payment request with id :{request.PaymentId}, is not exist" });
            }

            if(paymentRequest.State != PaymentStatus.Waiting)
            {

                var errorInfo = new ErrorInfo
                {
                    Message = $"Invalid payment request state {paymentRequest.State}.Payment request state should be" +
                    $"in  {PaymentStatus.Waiting}"
                };

                return Failure(HttpStatusCode.BadRequest, errorInfo);
       
            }

            var unitResult = await _paymentMethodResolver.Resolve(request.PaymentGatewayName, cancellationToken);

            if (unitResult.IsFailure)
            {
                return unitResult.ConvertFaildUnitResult<PaymentProcessResultDto>();
            }

            var paymentMethod = unitResult.Value;

            return  await  paymentMethod.Process(request.PaymentId, new ProcessPaymentModel
            {
                ReturnUrl = request.ReturnUrl,
                CancelUrl = request.CancelUrl
            },cancellationToken);

        }
    }
}
