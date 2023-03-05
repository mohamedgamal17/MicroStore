using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Orders
{
    public class IndexModel : PageModel
    {
        public PagedList<OrderList> Orders { get; set; }
        public void OnGet()
        {
        }
    }
}
