using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Shipments
{
    public class DetailModel : PageModel
    {
        public Shipment Shipment { get; set; }
        public void OnGet()
        {
        }
    }
}
