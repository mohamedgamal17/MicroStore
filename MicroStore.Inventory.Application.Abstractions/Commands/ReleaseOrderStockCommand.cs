#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Common;
namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class ReleaseOrderStockCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}
