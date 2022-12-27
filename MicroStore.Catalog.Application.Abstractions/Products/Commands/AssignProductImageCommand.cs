using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class AssignProductImageCommand : ICommand
    {
        public Guid ProductId { get; set; }
        public ImageModel ImageModel { get; set; }
        public int DisplayOrder { get; set; }
    }
}
