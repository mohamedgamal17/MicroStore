using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class RefundPaymentRequestCommand : ICommand<PaymentRequestDto>
    {
        public Guid PaymentId { get; set; }
    }
}
