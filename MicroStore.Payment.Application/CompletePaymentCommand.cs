#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Domain.Shared.Dtos;
namespace MicroStore.Payment.Application
{
    public class CompletePaymentRequestCommand : ICommand<PaymentRequestCompletedDto>
    {
        public string  PaymentGatewayName { get; set; }
        public string Token { get; set; }
    }
}
