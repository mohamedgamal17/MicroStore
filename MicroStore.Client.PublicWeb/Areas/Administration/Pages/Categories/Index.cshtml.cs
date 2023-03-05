using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Categories
{
    public class IndexModel : PageModel
    {
        public List<Category>? Categories { get; set; }
        public void OnGet()
        {

        }
    }
}
