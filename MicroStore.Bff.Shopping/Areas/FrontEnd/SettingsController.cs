using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.Billing;
using MicroStore.Bff.Shopping.Data.Shipping;
using MicroStore.Bff.Shopping.Services.Billing;
using MicroStore.Bff.Shopping.Services.Shipping;

namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/settings")]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly PaymentSystemService _paymentSystemService;

        private readonly ShippingSystemService _shippingSystemService;
        public SettingsController(PaymentSystemService paymentSystemService, ShippingSystemService shippingSystemService)
        {
            _paymentSystemService = paymentSystemService;
            _shippingSystemService = shippingSystemService;
        }


        [Route("billing/systems")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(List<PaymentSystem>))]
        public async Task<IActionResult> ListPaymentSystems()
        {
            var result = await _paymentSystemService.ListAsync();

            return Ok(result);
        }

        [Route("shipping/systems")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<ShippingSystem>))]
        public async Task<IActionResult> ListShippingSystems()
        {
            var result = await _shippingSystemService.ListAsync();

            return Ok(result);
        }
    }
}
