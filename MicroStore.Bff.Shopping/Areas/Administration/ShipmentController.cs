using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Infrastructure;
using MicroStore.Bff.Shopping.Services.Shipping;
using MicroStore.Bff.Shopping.Data.Shipping;
using MicroStore.Bff.Shopping.Models.Shipping;
namespace MicroStore.Bff.Shopping.Areas.Administration
{
    [ApiExplorerSettings(GroupName = "Administration")]
    [Route("api/administration/shipments")]
    [ApiController]
    public class ShipmentController : Controller
    {
        private readonly ShipmentService _shipmentService;

        public ShipmentController(ShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<Shipment>))]
        public async Task<ActionResult<PagedList<Shipment>>> ListAsync(string? userId = null,string? orderNumber = null, string? trackingNumber = null, int status = -1, string? country = null, DateTime? startDate = null, DateTime? endDate = null, int skip = 0, int length = 10, string? sortBy = null, bool desc = false)
        {

            var response = await _shipmentService.ListAsync(userId, orderNumber, trackingNumber, status, country, startDate, endDate, skip, length, sortBy, desc);

            return response;
        }


        [Route("{shipmentId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Shipment))]
        public async Task<ActionResult<Shipment>> GetShipmentAsync(string shipmentId)
        {
            var response = await _shipmentService.GetAsync(shipmentId);

            return response;
        }

        [Route("{shipmentId}/rates")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ShipmentRate>))]
        public async Task<ActionResult<List<ShipmentRate>>> GetShipmentRates(string shipmentId)
        {
            var response = await _shipmentService.RetriveShipmentRatesAsync(shipmentId);

            return response;
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Shipment))]
        public async Task<ActionResult<Shipment>> CreateShipmentAsync(ShipmentModel model)
        {
            var response = await _shipmentService.CreateAsync(model);

            return CreatedAtAction(nameof(GetShipmentAsync), new { shipmentId = response.Id }, response);
        }

        [Route("/fullfill/{shipmentId}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Shipment))]
        public async Task<ActionResult<Shipment>> FullfillAsync(string shipmentId,  FullfillModel model)
        {
            var response = await _shipmentService.FullfillAsync(shipmentId, model);

            return Ok(response);
        }

        [Route("/buylable/{shipmentId}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Shipment))]
        public async Task<ActionResult<Shipment>> BuyLabelAsync(string shipmentId, BuyShipmentLabelModel model)
        {
            var response = await _shipmentService.BuyLabelAsync(shipmentId, model);

            return Ok(response);
        }
    }
}
