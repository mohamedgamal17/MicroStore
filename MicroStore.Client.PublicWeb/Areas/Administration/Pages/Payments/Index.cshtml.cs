using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Payments
{
    public class IndexModel : PageModel
    {

        public PagedList<PaymentRequestList> PaymentRequests { get; set; }

        public void OnGet()
        {
        }
    }
}
