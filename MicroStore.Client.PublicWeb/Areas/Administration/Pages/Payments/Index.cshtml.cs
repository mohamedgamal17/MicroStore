#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Payments
{
    public class IndexModel : PageModel
    {
        public PagedList<PaymentRequestList> PaymentRequests { get; set; }

        private readonly PaymentRequestService _paymentRequestService;

        public IndexModel(PaymentRequestService paymentRequestService)
        {
            _paymentRequestService = paymentRequestService;
        }

        public async Task<IActionResult> OnGet()
        {
            PaymentRequests = await _paymentRequestService.ListAsync(new PagingReqeustOptions { PageNumber = 1, PageSize = 10 });

            return Page();
        }
    }
}
