using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
namespace MicroStore.Payment.Application.Commands
{
    public class ProcessPaymentRequestCommandHandler : CommandHandler<ProcessPaymentRequestCommand, PaymentProcessResultDto>
    {
        private readonly IPaymentMethodResolver _paymentMethodResolver;

        public ProcessPaymentRequestCommandHandler(IPaymentMethodResolver paymentMethodResolver)
        {
            _paymentMethodResolver = paymentMethodResolver;
        }

        public override  Task<PaymentProcessResultDto> Handle(ProcessPaymentRequestCommand request, CancellationToken cancellationToken)
        {
            var paymentMethod = _paymentMethodResolver.Resolve(request.PaymentGatewayName);

            return  paymentMethod.Process(request.PaymentId, new ProcessPaymentModel
            {
                ReturnUrl = request.ReturnUrl,
                CancelUrl = request.CancelUrl
            },cancellationToken);
        }
    }
}
