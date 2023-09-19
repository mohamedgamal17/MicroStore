using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using System.Net;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Components.ProductDetailsWidget
{
    [Widget(AutoInitialize = true,
        ScriptFiles = new string[] { "/Pages/Shared/Components/ProductDetailsWidget/product-details-widget.js" },
        StyleFiles = new string[] { "/Pages/Shared/Components/ProductDetailsWidget/product-details-widget.css" })]
    public class ProductDetailsWidgetViewComponent : AbpViewComponent
    {
        private readonly ProductService _productService;

        private readonly ProductReviewService _productReviewService;
        public ProductDetailsWidgetViewComponent(ProductService productService, ProductReviewService productReviewService)
        {
            _productService = productService;
            _productReviewService = productReviewService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string productId)
        {
            var product = await _productService.GetAsync(productId);

            var productReveiwsResponse = await _productReviewService.ListAsync(productId, new PagingReqeustOptions { Length = 10, Skip = 0 });

            var model = new ProductDetailsWidgetViewModel
            {
                Product = product,
                ProductReviews = productReveiwsResponse.Items
            };


            return View(model);

        }
    }

    public class ProductDetailsWidgetViewModel
    {
        public Product Product { get; set; }

        public List<ProductReview> ProductReviews { get; set; }
    }
}
