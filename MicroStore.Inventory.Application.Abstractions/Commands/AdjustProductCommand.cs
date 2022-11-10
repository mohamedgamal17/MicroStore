using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class AdjustProductCommand : ICommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
