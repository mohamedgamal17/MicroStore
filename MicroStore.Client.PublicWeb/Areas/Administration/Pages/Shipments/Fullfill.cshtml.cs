using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Shipments;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;
using NUglify.Helpers;
using System.Net;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Shipments
{
    public class FullfillModel : PageModel
    {
        [BindProperty]
        public ShipmentPackageModel ShipmentPackageModel { get; set; }
        public Shipment Shipment { get; set; }

        private readonly ShipmentService _shipmentService;

        public FullfillModel(ShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        public async Task<IActionResult> OnGet(string shipmentId)
        {
            await PreapreShipment(shipmentId);

            if (Shipment.Status != ShipmentStatus.Created)
            {
                return RedirectToPage("index");
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string shipmentId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await PreapreShipment(shipmentId);

            var requestOptions = new ShipmentFullfillRequestOptions
            {

                    Dimension = new Dimension
                    {
                        Height = ShipmentPackageModel.Dimension.Height,
                        Lenght = ShipmentPackageModel.Dimension.Lenght,
                        Width = ShipmentPackageModel.Dimension.Width,
                        Unit = ShipmentPackageModel.Dimension.Unit,
                    },

                    Weight = new Weight
                    {
                        Value = ShipmentPackageModel.Weight.Value,
                        Unit = ShipmentPackageModel.Weight.Unit
                    }
         
            };


            try
            {
                await _shipmentService.FullfillAsync(shipmentId, requestOptions);

                return RedirectToPage("BuyLabel");
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {

                if (ex.Erorr.Title != null) ModelState.AddModelError("", ex.Erorr.Title);
                if (ex.Erorr.Type != null) ModelState.AddModelError("", ex.Erorr.Type);
                if (ex.Erorr.Detail != null) ModelState.AddModelError("", ex.Erorr.Detail);
                if (ex.Erorr.Errors != null)
                {
                    ex.Erorr.Errors.ForEach(error => ModelState.AddModelError("", string.Format("{0}:{1}", error.Key, error.Value.JoinAsString(" , "))));
                }

                await PreapreShipment(shipmentId);

                return Page();

            }
        }


        private async Task PreapreShipment(string shipmentId)
        {
            Shipment = await _shipmentService.GetAsync(shipmentId);
        }
    }
}
