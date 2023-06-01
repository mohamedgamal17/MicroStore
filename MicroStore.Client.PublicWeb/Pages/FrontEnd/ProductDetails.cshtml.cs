using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;

namespace MicroStore.Client.PublicWeb.Pages.FrontEnd
{
    public class ProductDetailsModel : PageModel
    {
        private readonly ILogger<ProductDetailsModel> _logger;

        private readonly ProductService _productService;

        public Product Product { get; set; }
        public ProductDetailsModel(ILogger<ProductDetailsModel> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }



        public async Task<IActionResult> OnGet(string id)
        {

            Product = await _productService.GetAsync(id);

            return Page();
        }
    }
}
