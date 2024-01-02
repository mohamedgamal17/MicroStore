using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Catalog.Application.Abstractions.Products
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
