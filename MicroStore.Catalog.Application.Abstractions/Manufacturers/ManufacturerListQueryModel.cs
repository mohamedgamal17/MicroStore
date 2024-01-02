using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Catalog.Application.Abstractions.Manufacturers
{
    public class ManufacturerListQueryModel : SortingQueryParams
    {
        public string? Name { get; set; }
    }
}
