using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class AssignProductCategoryCommand : ICommandV1
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsFeatured { get; set; }

    }
}
