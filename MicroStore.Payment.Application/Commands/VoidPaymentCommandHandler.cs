using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Domain;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.Commands
{
    public class VoidPaymentCommandHandler : CommandHandler<VoidPaymentCommand>
    {
        private readonly IRepository<PaymentRequest> _paymentRequestRepository;

        public VoidPaymentCommandHandler(IRepository<PaymentRequest> paymentRequestRepository)
        {
            _paymentRequestRepository = paymentRequestRepository;
        }

        public override async Task<Unit> Handle(VoidPaymentCommand request, CancellationToken cancellationToken)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository.SingleAsync(x => x.Id== request.PaymentId);

            paymentRequest.VoidPayment(request.FaultDate);

            await _paymentRequestRepository.UpdateAsync(paymentRequest);

            return Unit.Value;
        }
    }
}
