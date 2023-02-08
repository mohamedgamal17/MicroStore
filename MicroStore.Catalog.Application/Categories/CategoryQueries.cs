using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Application.Dtos;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Categories
{
    public class GetCategoryListQuery : SortingQueryParams, IQuery<ListResultDto<CategoryListDto>>
    {
    }
    public class GetCategoryQuery : IQuery<CategoryDto>
    {
        public Guid Id { get; set; }
    }

}
