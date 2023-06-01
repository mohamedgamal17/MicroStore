using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Components.ProductListWidget
{
    [Widget(AutoInitialize = true,
        ScriptFiles =new string[] { "/Pages/Shared/Components/ProductListWidget/product-list-widget.js" },
        StyleFiles = new string[] { "/Pages/Shared/Components/ProductListWidget/product-list-widget.css" })]
    public class ProductListWidgetViewComponent : AbpViewComponent
    {
        private readonly ProductService _productService;

        public ProductListWidgetViewComponent(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string pagingUrl,int currentPage = 1 ,int pageSize = 12,bool isFeatured = false,string? category = null,string? manufacturer = null, int productCardSize = 4)
        {
            var requestOptions = new PagingAndSortingRequestOptions
            {
                Skip = (currentPage - 1) * pageSize,
                Lenght = pageSize,
            };


            var result = await _productService.ListAsync(requestOptions);

            var model = new ProductListWidgetModel
            {
                Products = result.Items,
                ProductCardSize = productCardSize,
                Pager = new PagerModel(result.TotalCount, pageSize, result.PageNumber, pageSize, pagingUrl)
            };

            return View(model);
        }
    }

    public class ProductListWidgetModel
    {
        public List<Product> Products { get; set; }

        public int ProductCardSize { get; set; }
        public PagerModel Pager { get; set; }
    }
}