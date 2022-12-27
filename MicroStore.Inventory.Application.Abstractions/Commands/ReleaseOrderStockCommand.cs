#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class ReleaseOrderStockCommand : ICommandV1
    {
        public string ExternalOrderId { get; set; }

    }
}
