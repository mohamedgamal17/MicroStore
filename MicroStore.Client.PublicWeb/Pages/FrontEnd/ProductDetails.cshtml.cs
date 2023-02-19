using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.BuildingBlocks.Results.Http;
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



        public async Task<IActionResult> OnGet(Guid id)
        {

            var response = await _productService.RetriveAsync(id);

            if (response.IsFailure)
            {
                TempData.Put("ErrorInfo", response.HttpEnvelopeResult?.Error);

                TempData["StatusCode"] = response.HttpStatusCode;

                return Redirect("~/frontend/error");
            }

            Product = response.Result;

            return Page();
        }
    }
}
