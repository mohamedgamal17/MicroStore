using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public class UpdatePaymentSystemCommand : ICommand<PaymentSystemDto>
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
