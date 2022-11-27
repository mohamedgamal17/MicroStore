using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;

namespace MicroStore.Payment.Application.Commands
{
    public class CompletePaymentRequestCommandHandler : CommandHandler<CompletePaymentRequestCommand, PaymentRequestCompletedDto>
    {

        private readonly IPaymentMethodResolver _paymentResolver;

        public CompletePaymentRequestCommandHandler(IPaymentMethodResolver paymentResolver)
        {
            _paymentResolver = paymentResolver;
        }

        public override Task<PaymentRequestCompletedDto> Handle(CompletePaymentRequestCommand request, CancellationToken cancellationToken)
        {
            var paymentMethod = _paymentResolver.Resolve(request.PaymentGatewayName);

            return paymentMethod.Complete(new CompletePaymentModel
            {
                Token = request.Token,
            },cancellationToken);
        }
    }
}
