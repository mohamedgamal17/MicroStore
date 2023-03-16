using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly CategoryService _categoryService;

        public List<Category>? Categories { get; set; }
        public IndexModel(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public async Task<IActionResult> OnGet()
        {
            Categories = await _categoryService
                .ListAsync(new SortingRequestOptions ());

            return Page();
        }
    }
}
