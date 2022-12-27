using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class RefundPaymentRequestCommand : ICommandV1
    {
        public Guid PaymentId { get; set; }
    }
}
