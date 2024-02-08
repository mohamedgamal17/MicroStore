using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Shipping;
using MicroStore.Bff.Shopping.Infrastructure;
using MicroStore.Bff.Shopping.Services.Shipping;

namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/user/shipments")]
    [ApiController]
    [Authorize]
    public class UserShipmentController : Controller
    {
        private readonly IWorkContext _workContext;

        private readonly ShipmentService _shipmentService;
        public UserShipmentController(IWorkContext workContext, ShipmentService shipmentService)
        {
            _workContext = workContext;
            _shipmentService = shipmentService;
        }
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<Shipment>))]
        public async Task<ActionResult<PagedList<Shipment>>> ListAsync(string orderNumber = "", string trackingNumber = "", int status = -1, string country = "", DateTime startDate = default, DateTime endDate = default, int skip = 0, int length = 10, string sortBy = "", bool desc = false)
        {
            var userId = _workContext.User!.Id;

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
    }
}
