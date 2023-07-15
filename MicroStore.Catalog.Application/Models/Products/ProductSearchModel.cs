using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Catalog.Application.Models.Products
{
    public class ProductSearchModel : PagingQueryParams
    {
        public string KeyWords { get; set; }
    }

    public class ProductSearchBySkuModel : PagingQueryParams
    {
        public string Sku { get; set; }
    }
}
