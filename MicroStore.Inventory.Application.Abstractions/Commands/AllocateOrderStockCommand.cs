#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Abstractions.Common;
namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class AllocateOrderStockCommand : ICommand<Result>
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}
