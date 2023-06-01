using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
namespace MicroStore.Client.PublicWeb.Pages
{
    public class ProductsModel : PageModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 24;
        public string? Category { get; set; }
        public string? Manufacturer { get; set; }
        public ProductsModel()
        {
        }

        public IActionResult OnGet(int currentPage = 1, string category = null, string manufacturer = null)
        {
            CurrentPage = currentPage;
            Category = category;
            Manufacturer = manufacturer;
            return Page();
        }
    }
}
