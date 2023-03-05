using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
namespace MicroStore.Client.PublicWeb.Pages.FrontEnd
{
    public class ProductsModel : PageModel
    {
        private readonly ProductService _productService;

        private readonly ILogger<ProductsModel> _logger;
        public PagedList<ProductList> Products { get; set; }

        public ProductsModel(ProductService productService, ILogger<ProductsModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(PagingAndSortingParamsQueryString @params)
        {
            var options = new PagingAndSortingRequestOptions
            {
                PageNumber = @params.PageNumber,
                PageSize = @params.PageSize,
                SortBy = @params.SortBy,
                Desc = @params.Desc
            };

            var response = await _productService.ListAsync(options);

            Products = response;

            return Page();
        }
    }
}
