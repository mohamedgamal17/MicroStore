#nullable disable
using MicroStore;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Domain.Shared.Dtos;
namespace MicroStore.Payment.Application.Commands.Requests
{
    public class CompletePaymentRequestCommand : ICommand<PaymentRequestCompletedDto>
    {
        public string PaymentGatewayName { get; set; }
        public string Token { get; set; }
    }
}
