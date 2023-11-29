using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Components.ProductImageSearchWidget
{
    [Widget(
            AutoInitialize = true,
            ScriptFiles = new string[] { "/Pages/Shared/Components/ProductImageSearchWidget/product-image-search-widget.js" }
        )]
    public class ProductImageSearchWidgetViewComponent  : AbpViewComponent
    {

        private readonly IObjectStorageProvider _objectStorageProvider;

        private readonly ProductService _productService;

        private readonly BasketService _basketService;

        private readonly IWorkContext _workContext;
        public ProductImageSearchWidgetViewComponent(IObjectStorageProvider objectStorageProvider, ProductService productService, BasketService basketService, IWorkContext workContext)
        {
            _objectStorageProvider = objectStorageProvider;
            _productService = productService;
            _basketService = basketService;
            _workContext = workContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(string image)
        {
            var imageStream = await _objectStorageProvider.GetAsync(image);

            if(imageStream != null)
            {
                var requestOptions = new ProductSearchByImageRequestOptions
                {
                    Image = await imageStream.GetAllBytesAsync()
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
            else
            {
                var model = new ProductImageSearchWidgetViewComponentModel
                {
                    Error = true,

                    ErrorMessage = "Image is not exist"
                };

                return View(model);
             
            }

        }
    }

    public class ProductImageSearchWidgetViewComponentModel
    {
        public List<Product> Products { get; set; }
        public string ImagePath { get; set; }
        public MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart.Basket UserBasket { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}
