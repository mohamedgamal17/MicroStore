#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Payments
{
    public class DetailModel : PageModel
    {
        public PaymentRequest PaymentRequest { get; set; }

        private readonly PaymentRequestService _paymentRequestService;

        public DetailModel(PaymentRequestService paymentRequestService)
        {
            _paymentRequestService = paymentRequestService;
        }

        public async Task<IActionResult> OnGet(string paymentRequestId)
        {
            PaymentRequest = await _paymentRequestService.GetAsync(paymentRequestId);

            return Page();
        }
    }
}
