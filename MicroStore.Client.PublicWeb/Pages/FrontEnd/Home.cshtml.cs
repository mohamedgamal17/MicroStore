using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using Microsoft.Extensions.Logging;

namespace MicroStore.Client.PublicWeb.Pages.FrontEnd
{
    public class HomeModel : PageModel
    {
        private readonly ILogger<HomeModel> _logger;

        private readonly ProductService _productService;
        public PagedList<Product> Products { get; set; }
        public HomeModel(ILogger<HomeModel> logger,
            ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> OnGet(PagingAndSortingParamsQueryString @params)
        {
            try
            {
                var pagingOptions = new PagingAndSortingRequestOptions
                {
                    Skip = @params.Skip,
                    Lenght = @params.Lenght,
                    Desc = @params.Desc
                };


                Products = await _productService.ListAsync(pagingOptions);

                return Page();
            }
            catch(Exception ex)
            {
                Products = new PagedList<Product>() { Items = new List<Product>() };

                _logger.LogException(ex);
                return Page();
            }
   

        }
    }
}
