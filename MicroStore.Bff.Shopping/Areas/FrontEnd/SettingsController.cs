using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.Billing;
using MicroStore.Bff.Shopping.Services.Billing;

namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/settings")]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly PaymentSystemService _paymentSystemService;

        public SettingsController(PaymentSystemService paymentSystemService)
        {
            _paymentSystemService = paymentSystemService;
        }


        [Route("billing/systems")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(List<PaymentSystem>))]
        public async Task<IActionResult> ListPaymentSystems()
        {
            var result = await _paymentSystemService.ListAsync();

            return Ok(result);
        }
    }
}
