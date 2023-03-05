using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Orders
{
    public class DetailModel : PageModel
    {

        public Order Order { get; set; }
        public void OnGet()
        {
        }
    }
}
