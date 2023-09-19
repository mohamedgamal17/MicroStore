using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Components.ProductReviewListWidget
{
    [Widget(AutoInitialize = true)]
    public class ProductReviewListWidgetViewComponent : AbpViewComponent
    {
        private readonly ProductReviewService _productReveiwService;

        public ProductReviewListWidgetViewComponent(ProductReviewService productReveiwService)
        {
            _productReveiwService = productReveiwService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string productId , int currentPage = 1 , int pageSize = 10)
        {
            var pagingOptions = new ProductReviewListRequestOption
            {
                Length = 10,
                Skip = (currentPage - 1) * pageSize
            };

            var response = await _productReveiwService.ListAsync(productId, pagingOptions);

            var url = QueryHelpers.AddQueryString("/ProductReviews", "id", productId);

            var model = new ProductReviewListWidgetViewComponentModel
            {
                ProductReviews = response.Items,
                Pager = new PagerModel(response.TotalCount, pageSize, response.PageNumber, pageSize, url)
            };

            return View(model);
        }
    }

    public class ProductReviewListWidgetViewComponentModel
    {
        public List<ProductReview> ProductReviews { get; set; }
        public PagerModel Pager { get; set; }
    }

}
