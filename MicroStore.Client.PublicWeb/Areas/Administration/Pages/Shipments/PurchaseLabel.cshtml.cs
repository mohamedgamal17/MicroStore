using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;
using NUglify.Helpers;
using System.Net;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Shipments
{
    public class PurchaseLabelModel : PageModel
    {
        public List<ShipmentRate> ShipmentRates { get; set; }

        public Order Order { get; set; }

        public Shipment Shipment { get; set; }

        [BindProperty]
        public string ShipmentRateId { get; set; }


        private readonly ShipmentService _shipmentService;

        private readonly OrderService _orderService;

        public PurchaseLabelModel(ShipmentService shipmentService, OrderService orderService)
        {
            _shipmentService = shipmentService;
            _orderService = orderService;
        }

        public async Task<IActionResult> OnGet(string shipmentId)
        {
            await BuildViewModel(shipmentId);

            await PrepareShipmentRates(shipmentId);

            return Page();
        }


        public async Task<IActionResult> OnPost(string shipmentId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var requestOptions = new PurchaseLabelRequestOptions { ShipmentRateId = ShipmentRateId };

            try
            {
                await _shipmentService.PurchaseLabelAsync(shipmentId, requestOptions);

                return RedirectToPage("Index");
            }
            catch(MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                if (ex.Erorr.Title != null) ModelState.AddModelError("", ex.Erorr.Title);
                if (ex.Erorr.Type != null) ModelState.AddModelError("", ex.Erorr.Type);
                if (ex.Erorr.Detail != null) ModelState.AddModelError("", ex.Erorr.Detail);
                if (ex.Erorr.Errors != null)
                {
                    ex.Erorr.Errors.ForEach(error => ModelState.AddModelError("", string.Format("{0}:{1}", error.Key, error.Value.JoinAsString(" , "))));
                }

                await BuildViewModel(shipmentId);

                await PrepareShipmentRates(shipmentId);

                return Page();
            }
        }


        private async Task BuildViewModel(string shipmentId)
        {
            Shipment = await _shipmentService.GetAsync(shipmentId);

            Order = await _orderService.GetAsync(Guid.Parse(Shipment.OrderId));
        }
        private async Task PrepareShipmentRates(string shipmentId)
        {
            ShipmentRates = await _shipmentService.RetrieveRatesAsync(shipmentId);
        }

       
    }
}
