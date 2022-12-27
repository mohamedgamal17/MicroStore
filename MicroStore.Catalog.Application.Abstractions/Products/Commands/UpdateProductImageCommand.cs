using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class UpdateProductImageCommand : ICommandV1
    {
        public Guid ProductId { get; set; }
        public Guid ProductImageId { get; set; }
        public int DisplayOrder { get; set; }

    }
}
