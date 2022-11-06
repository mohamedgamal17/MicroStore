using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;


namespace MicroStore.Catalog.Application.Abstractions.Categories.Queries
{
    public class GetCategoryListQuery : IQuery<List<CategoryDto>>
    {
    }
}
