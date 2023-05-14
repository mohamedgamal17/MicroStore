using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;

namespace MicroStore.Client.PublicWeb.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ProductService _productService;
        //public async Task<IActionResult> Index(PagingAndSortingParamsQueryString @params)
        //{
        //    var pagingOptions = new PagingAndSortingRequestOptions
        //    {
        //        Skip = @params.Skip,
        //        Lenght = @params.Lenght,
        //        Desc = @params.Desc
        //    };


        //    Products = await _productService.ListAsync(pagingOptions);


        //    return View();
        //}
    }
}
