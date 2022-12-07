using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class AssignProductImageCommand : ICommand<ProductDto>
    {
        public Guid ProductId { get; set; }
        public ImageModel ImageModel { get; set; }
        public int DisplayOrder { get; set; }
    }
}
