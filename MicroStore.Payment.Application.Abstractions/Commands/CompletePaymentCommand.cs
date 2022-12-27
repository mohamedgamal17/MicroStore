#nullable disable
using MicroStore;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;
namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class CompletePaymentRequestCommand : ICommandV1
    {
        public string PaymentGatewayName { get; set; }
        public string Token { get; set; }
    }
}
