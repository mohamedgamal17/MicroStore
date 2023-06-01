using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using Microsoft.Extensions.Logging;

namespace MicroStore.Client.PublicWeb.Pages
{
    public class HomeModel : PageModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 24;
        public IActionResult OnGet(int currentPage = 1)
        {
            CurrentPage = currentPage;

            return Page();
        }
    }
}
