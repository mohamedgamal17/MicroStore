using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;

namespace MicroStore.Client.PublicWeb.Pages
{
    public class ProductDetailsModel : PageModel
    {
        public string ProductId { get; set; }

        public IActionResult OnGet(string id)
        {
            ProductId = id;

            return Page();
        }
    }
}
