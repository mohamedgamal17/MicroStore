using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Payments
{
    public class DetailModel : PageModel
    {

        public PaymentRequest PaymentRequest { get; set; }
        public void OnGet()
        {
        }
    }
}
