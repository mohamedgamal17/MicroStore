#nullable disable
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Shipments
{
    public class IndexModel : PageModel
    {
        public PagedList<ShipmentList> Shipments { get; set; }

        private readonly ShipmentService _shipmentService;

        public IndexModel(ShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        public async Task<IActionResult> OnGet()
        {
            Shipments = await _shipmentService.ListAsync(new PagingReqeustOptions { PageNumber = 1, PageSize = 10 });

            return Page();

        }
    }
}
