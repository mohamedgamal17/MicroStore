#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Dtos;
namespace MicroStore.Payment.Application.Commands.Requests
{
    public class CreatePaymentRequestCommand : ICommand<PaymentCreatedDto>
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }

    }
}
