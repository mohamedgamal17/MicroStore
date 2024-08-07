using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
namespace MicroStore.Client.PublicWeb.Pages.Products
{
    public class ReviewsModel : PageModel
    {
        private readonly ProductService _productService;
        public Product Product { get; set; }
        public int CurrentPage { get; set; }
        public ReviewsModel(ProductService productService)
        {
            _productService = productService;
        }


        public async Task<IActionResult> OnGetAsync(string id, int currentPage = 1)
        {

            Product = await _productService.GetAsync(id);

            CurrentPage = currentPage;

            return Page();
        }
    }
}
