using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Products
{
    public class IndexModel : PageModel
    {
        public List<ProductList>? Products { get; set; }
        public void OnGet()
        {
        }
    }
}
