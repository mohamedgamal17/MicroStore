using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Catalog.Application.Models.Products
{
    public class ProductListQueryModel  : PagingAndSortingQueryParams
    {
        public string? Categories { get; set; }
        public string? Manufacturers { get; set; }
        public string? Tags { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }
}
