using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Products
{
    public class IndexModel : PageModel
    {
        public PagedList<Product> Products { get; set; }

        private readonly ProductService _productService;
        public IndexModel(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> OnGet()
        {
            Products = await _productService.ListAsync(new PagingAndSortingRequestOptions { PageNumber = 1 , PageSize = 10});

            return Page();
        }
    }
}
