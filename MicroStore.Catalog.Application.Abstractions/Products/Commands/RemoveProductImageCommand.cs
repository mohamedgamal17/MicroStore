using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class RemoveProductImageCommand : ICommand
    {
        public Guid ProductId { get; set; }
        public Guid ProductImageId { get; set; }

    }
}
