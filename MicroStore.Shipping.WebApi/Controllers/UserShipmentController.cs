using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.BuildingBlocks.Security;
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
        

        private readonly IApplicationCurrentUser _applicationCurrentUser;

        private readonly IShipmentQueryService _shipmentQueryService;

        public UserShipmentController(IApplicationCurrentUser applicationCurrentUser, IShipmentQueryService shipmentQueryService)
        {
            _applicationCurrentUser = applicationCurrentUser;
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

            var result = await _shipmentQueryService.ListAsync(queryParams, _applicationCurrentUser.Id);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("{shipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]

        public async Task<IActionResult> RetriveShipmentById(string shipmentId)
        {

            var result = await _shipmentQueryService.GetAsync(shipmentId);

            return FromResultV2(result, HttpStatusCode.OK);
        }


        [HttpGet]
        [Route("order_id/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]

        public async Task<IActionResult> RetriveShipmentByOrderId(string orderId)
        {

            var result = await _shipmentQueryService.GetByOrderIdAsync(orderId);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("order_number/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]

        public async Task<IActionResult> RetriveShipmentByOrderNumber(string orderNumber)
        {

            var result = await _shipmentQueryService.GetByOrderNumberAsync(orderNumber);

            return FromResultV2(result, HttpStatusCode.OK);
        }
    }
}
