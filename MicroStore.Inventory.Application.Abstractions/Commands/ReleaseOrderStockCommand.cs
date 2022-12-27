#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class ReleaseOrderStockCommand : ICommand
    {
        public string ExternalOrderId { get; set; }

    }
}
