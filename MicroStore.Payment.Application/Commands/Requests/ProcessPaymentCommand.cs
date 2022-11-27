#nullable disable
using MicroStore;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Domain.Shared.Dtos;
namespace MicroStore.Payment.Application.Commands.Requests
{
    public class ProcessPaymentRequestCommand : ICommand<PaymentProcessResultDto>
    {
        public Guid PaymentId { get; set; }
        public string PaymentGatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
