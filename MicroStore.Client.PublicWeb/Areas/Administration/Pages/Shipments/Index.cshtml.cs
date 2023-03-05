using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Shipments
{
    public class IndexModel : PageModel
    {

        public PagedList<ShipmentList> Shipments { get; set; }

        public void OnGet()
        {


        }
    }
}
