using MicroStore.BuildingBlocks.Utils.Paging.Params;

namespace MicroStore.Catalog.Application.Models.ProductReviews
{
    public class ProductReviewListQueryModel : PagingQueryParams
    {
        public string? UserId { get; set; }
    }
}
