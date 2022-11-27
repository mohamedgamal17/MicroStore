using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Payment.Application.Commands.Requests
{
    public class RefundPaymentRequestCommand : ICommand
    {
        public Guid PaymentId { get; set; }
    }
}
