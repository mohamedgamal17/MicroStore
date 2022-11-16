using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Payment.Application.Commands.Requests
{
    public class VoidPaymentCommand : ICommand
    {
        public Guid PaymentId { get; set; }
        public DateTime FaultDate { get; set; }
    }
}
