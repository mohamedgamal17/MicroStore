using MicroStore.BuildingBlocks.Utils.Paging.Params;

namespace MicroStore.Catalog.Application.Abstractions.ProductReveiws
{
    public class ProductReviewListQueryModel : PagingQueryParams
    {
        public string? UserId { get; set; }
    }
}
