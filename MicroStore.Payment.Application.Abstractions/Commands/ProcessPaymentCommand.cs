#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class ProcessPaymentRequestCommand : ICommand
    {
        public Guid PaymentId { get; set; }
        public string PaymentGatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
