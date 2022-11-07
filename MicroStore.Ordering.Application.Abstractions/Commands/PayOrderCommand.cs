using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Application.Abstractions.Dtos;

namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class PayOrderCommand : ICommand<PaymentDto>
    {
        public Guid OrderId { get; set; }
    }
}
