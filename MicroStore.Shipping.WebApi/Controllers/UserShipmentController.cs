using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Shipments;
using MicroStore.Shipping.Domain.Security;
using System.Net;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/user/shipments")]
    public class UserShipmentController : MicroStoreApiController
    {
        private readonly IShipmentQueryService _shipmentQueryService;

        public UserShipmentController( IShipmentQueryService shipmentQueryService)
        {
   
            _shipmentQueryService = shipmentQueryService;
        }


        [HttpGet]
        [Route("")]
        [RequiredScope(ShippingScope.Shipment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ShipmentDto>))]

        public async Task<IActionResult> RetriveUserShipmentList( [FromQuery] PagingParamsQueryString @params)
        {
            var queryParams = new PagingQueryParams
            {
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber
            };

            var result = await _shipmentQueryService.ListAsync(queryParams, CurrentUser.Id.ToString()!);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{shipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]

        public async Task<IActionResult> RetriveShipmentById(string shipmentId)
        {

            var result = await _shipmentQueryService.GetAsync(shipmentId);

            return result.ToOk();
        }


        [HttpGet]
        [Route("order_id/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]

        public async Task<IActionResult> RetriveShipmentByOrderId(string orderId)
        {

            var result = await _shipmentQueryService.GetByOrderIdAsync(orderId);

            return result.ToOk();
        }

        [HttpGet]
        [Route("order_number/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]

        public async Task<IActionResult> RetriveShipmentByOrderNumber(string orderNumber)
        {

            var result = await _shipmentQueryService.GetByOrderNumberAsync(orderNumber);

            return result.ToOk();
        }
    }
}
