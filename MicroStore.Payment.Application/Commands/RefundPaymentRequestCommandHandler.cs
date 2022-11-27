using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Domain;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.Commands
{
    public class RefundPaymentRequestCommandHandler : CommandHandler<RefundPaymentRequestCommand>
    {
        private readonly IRepository<PaymentRequest> _paymentRequestRepository;

        private readonly IPaymentMethodResolver _paymentMethodResolver;
        public RefundPaymentRequestCommandHandler(IRepository<PaymentRequest> paymentRequestRepository, IPaymentMethodResolver paymentMethodResolver)
        {
            _paymentRequestRepository = paymentRequestRepository;
            _paymentMethodResolver = paymentMethodResolver;
        }

        public override async Task<Unit> Handle(RefundPaymentRequestCommand request, CancellationToken cancellationToken)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository
                .SingleAsync(x => x.Id == request.PaymentId);

            var paymentMethod = _paymentMethodResolver.Resolve(paymentRequest.PaymentGateway!);

            await paymentMethod.Refund(request.PaymentId, cancellationToken);

            return Unit.Value;
        }
    }
}
