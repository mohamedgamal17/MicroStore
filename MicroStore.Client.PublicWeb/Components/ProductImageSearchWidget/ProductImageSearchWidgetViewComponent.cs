using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.BlobStoring;

namespace MicroStore.Client.PublicWeb.Components.ProductImageSearchWidget
{
    [Widget(
            AutoInitialize = true,
            ScriptFiles = new string[] { "/Pages/Shared/Components/ProductImageSearchWidget/product-image-search-widget.js" }
        )]
    public class ProductImageSearchWidgetViewComponent  : AbpViewComponent
    {

        private readonly IBlobContainer<MultiMediaBlobContainer> _blobContainer;

        private readonly ProductService _productService;

        private readonly BasketService _basketService;

        private readonly IWorkContext _workContext;

        public ProductImageSearchWidgetViewComponent(IBlobContainer<MultiMediaBlobContainer> blobContainer, ProductService productService, BasketService basketService, IWorkContext workContext)
        {
            _blobContainer = blobContainer;
            _productService = productService;
            _basketService = basketService;
            _workContext = workContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(string image)
        {
            var imageStream = await _blobContainer.GetAllBytesAsync(image);

            var requestOptions = new ProductSearchByImageRequestOptions
            {
                Image = imageStream
            };

            var products = await _productService.SearchByImage(requestOptions);

            var userBasket = await _basketService.RetrieveAsync(_workContext.TryToGetCurrentUserId());

            var model = new ProductImageSearchWidgetViewComponentModel
            {
                Products = products,
                UserBasket = userBasket
            };

            return View(model);

        }
    }

    public class ProductImageSearchWidgetViewComponentModel
    {
        public List<Product> Products { get; set; }
        public MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart.Basket UserBasket { get; set; }
    }
}
