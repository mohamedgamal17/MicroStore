using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Inventory.Application.Models
{
    public class ProductSearchModel : PagingQueryParams
    {
        public string KeyWords { get; set; }
    }
}
