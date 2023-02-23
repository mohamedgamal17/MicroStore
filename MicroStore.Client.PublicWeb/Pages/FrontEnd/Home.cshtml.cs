using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;

namespace MicroStore.Client.PublicWeb.Pages.FrontEnd
{
    public class HomeModel : PageModel
    {
        private readonly ILogger<HomeModel> _logger;

        private readonly ProductService _productService;
        public PagedList<ProductList> Products { get; set; }
        public HomeModel(ILogger<HomeModel> logger,
            ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> OnGet(PagingAndSortingParamsQueryString @params)
        {
            var pagingOptions = new PagingAndSortingRequestOptions
            {
                PageNumber = @params.PageNumber,
                PageSize = @params.PageSize,
                Desc = @params.Desc
            };

            var result = await _productService.ListAsync(pagingOptions);

            if (result.IsFailure)
            {
                TempData["HttpEnvelopeResult"] = result.HttpEnvelopeResult;
                TempData["StatusCode"] = result.HttpStatusCode;

                _logger.LogInformation($"Status code  : {result.HttpStatusCode}");
                return Redirect("~/frontend/Error");
            }

            Products = result.Result;


            return Page();

        }
    }
}