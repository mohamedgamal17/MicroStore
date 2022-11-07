using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class ConfirmOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
    }
}
