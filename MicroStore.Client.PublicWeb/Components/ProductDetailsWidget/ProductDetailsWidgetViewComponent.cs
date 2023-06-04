using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
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
        public ProductDetailsWidgetViewComponent(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string productId)
        {
            try
            {
                var product = await _productService.GetAsync(productId);

                var model = new ProductDetailsWidgetViewModel
                {
                    Product = product,
                };

                return View(model);
            }
            catch(MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.NotFound)
            {
                return View(new ProductDetailsWidgetViewModel 
                { 
                    HasError = true ,
                    Error = ex.Erorr
                });
            }
         
        }
    }

    public class ProductDetailsWidgetViewModel
    {
        public Product Product { get; set; }
        public bool HasError { get; set; }

        public MicroStoreError Error{ get; set; }
    }
}
