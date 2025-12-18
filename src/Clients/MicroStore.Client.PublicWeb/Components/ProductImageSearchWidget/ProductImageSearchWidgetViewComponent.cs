using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
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
        public async Task<IViewComponentResult> InvokeAsync(string image, int currentPage = 1  , int pageSize = 24)
        {
            var requestOptions = new ProductSearchByImageRequestOptions
            {
                Image = await _objectStorageProvider.CalculatePublicReferenceUrl(image),
                Skip = (currentPage - 1) * pageSize,
                Length = pageSize,
            };

            var productResult = await _productService.SearchByImage(requestOptions);

            var userBasket = await _basketService.RetrieveAsync(_workContext.TryToGetCurrentUserId());

            Dictionary<string,string> queryDictionary = new Dictionary<string, string>();
            queryDictionary.Add(nameof(image), image);

            string url = QueryHelpers.AddQueryString("/Products", queryDictionary!);

            var model = new ProductImageSearchWidgetViewComponentModel
            {
                Products = productResult.Items,
                Pager = new PagerModel(productResult.TotalCount, pageSize, productResult.PageNumber, pageSize, url),
                UserBasket = userBasket
            };

            return View(model);

        }
    }

    public class ProductImageSearchWidgetViewComponentModel
    {
        public List<Product> Products { get; set; }
        public string ImagePath { get; set; }
        public PagerModel Pager { get; set; }
        public MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart.Basket UserBasket { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}
