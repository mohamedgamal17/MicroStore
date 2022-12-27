using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class RefundPaymentRequestCommand : ICommand
    {
        public Guid PaymentId { get; set; }
    }
}
