using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Catalog.Application.Abstractions.Products.Queries
{
    public class GetProductQuery : IQuery
    {
        public Guid Id { get; set; }
    }
}
