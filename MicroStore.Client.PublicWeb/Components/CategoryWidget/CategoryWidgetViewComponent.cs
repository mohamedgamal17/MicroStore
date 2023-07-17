using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Components.CategoryWidget
{
    [Widget(AutoInitialize = true)]
    public class CategoryWidgetViewComponent  : AbpViewComponent
    {
        private readonly CategoryService _categoryService;

        public CategoryWidgetViewComponent(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var requestOptions = new CategoryListRequestOptions();

            var categories = await _categoryService.ListAsync(requestOptions);


            var model = new CategoryWidgetModel
            {
                Categories = categories
            };

            return View(model);
        }
    }

    public class CategoryWidgetModel
    {
        public List<Category> Categories { get; set; }
    }
}
