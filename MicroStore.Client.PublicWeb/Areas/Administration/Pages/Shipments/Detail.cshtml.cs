#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Shipments
{
    public class DetailModel : PageModel
    {
        public ShipmentAggregate Shipment { get; set; }

        private readonly ShipmentAggregateService _shipmentAggregateService;

        public DetailModel(ShipmentAggregateService shipmentAggregateService)
        {
            _shipmentAggregateService = shipmentAggregateService;
        }

        public async Task<IActionResult> OnGet(string shipmentId)
        {
            Shipment = await _shipmentAggregateService.GetAsync(shipmentId);

            return Page();
        }
    }
}
