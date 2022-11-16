using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.Commands
{
    public class CreatePaymentRequestCommandHandler : CommandHandler<CreatePaymentRequestCommand, PaymentCreatedDto>
    {

        private readonly IRepository<PaymentRequest> _paymentRequestRepository;

        public CreatePaymentRequestCommandHandler(IRepository<PaymentRequest> paymentRequestRepository)
        {
            _paymentRequestRepository = paymentRequestRepository;
        }

        public override async Task<PaymentCreatedDto> Handle(CreatePaymentRequestCommand request, CancellationToken cancellationToken)
        {
            PaymentRequest paymentRequest = new PaymentRequest(request.OrderId, request.OrderNumber, request.CustomerId, request.Amount);

            await _paymentRequestRepository.InsertAsync(paymentRequest);

            return new PaymentCreatedDto
            {
                PaymentId = paymentRequest.Id,
                OrderId = paymentRequest.OrderId,
                OrderNumber = paymentRequest.OrderNumber,
                CustomerId = paymentRequest.CustomerId,
                Amount = paymentRequest.Amount,
                CreatedAt = paymentRequest.CreatedAt
            };
        }
    }
}
