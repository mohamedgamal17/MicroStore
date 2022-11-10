using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class ReciveProductCommand : ICommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
