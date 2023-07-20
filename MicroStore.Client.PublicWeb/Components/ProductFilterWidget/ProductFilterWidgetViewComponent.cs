using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Components.ProductFilterWidget
{
    [Widget(
        AutoInitialize =  true,
        ScriptFiles = new string[] { "/Pages/Shared/Components/ProductFilterWidget/product-filter-widget.js" }
        )]
    public class ProductFilterWidgetViewComponent : AbpViewComponent
    {
        private readonly CategoryService _categoryService;

        private readonly ManufacturerService _manufacturerService;

        public ProductFilterWidgetViewComponent(CategoryService categoryService, ManufacturerService manufacturerService)
        {
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? category , string? manufacturer)
        {
            var categoryRequestOptions = new CategoryListRequestOptions();

            var categories = await _categoryService.ListAsync(categoryRequestOptions);

            var manufacturerRequestOptions = new ManufacturerListRequestOptions();

            var manufacturers = await _manufacturerService.ListAsync(manufacturerRequestOptions);

            var model = new ProductFilterWidgetModel
            {
                Categories = categories,
                Manufacturers = manufacturers,
                FilterCategory = category,
                FilterManufacturer = manufacturer
            };

            return View(model);
        }
    }

    public class ProductFilterWidgetModel
    {
        public List<Category> Categories { get; set; }

        public List<Manufacturer> Manufacturers { get; set; }

        public string? FilterCategory { get; set; }

        public string? FilterManufacturer { get; set; }
    }
}
