using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public class ProductListQueryModel : PagingAndSortingQueryParams
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Manufacturer { get; set; }
        public string? Tag { get; set; }
        public double MinPrice { get; set; } = -1;
        public double MaxPrice { get; set; } = -1;
        public bool IsFeatured { get; set; }
    }
}
