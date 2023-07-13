using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Shipments;
using MicroStore.Shipping.Domain.Security;
namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/shipments")]
    public class ShipmentController : MicroStoreApiController
    {
        private readonly IShipmentCommandService _shipmentCommandService;

        private readonly IShipmentQueryService _shipmentQueryService;

        public ShipmentController(IShipmentCommandService shipmentCommandService, IShipmentQueryService shipmentQueryService)
        {
            _shipmentCommandService = shipmentCommandService;
            _shipmentQueryService = shipmentQueryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type= typeof(PagedResult<ShipmentDto>))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]

        public async Task<IActionResult> RetriveShipmentList([FromQuery] ShipmentListQueryModel queryParams,  string? userId=  null)
        {
            var result = await _shipmentQueryService.ListAsync(queryParams, userId);

            return result.ToOk();
        }




        [HttpGet]
        [Route("{shipmentId}")]
        [ActionName(nameof(RetriveShipmentById))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> RetriveShipmentById(string shipmentId)
        {

            var result = await _shipmentQueryService.GetAsync(shipmentId);

            return result.ToOk();
        }


        [HttpGet]
        [Route("order_id/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> RetriveShipmentByOrderId(string orderId)
        {

            var result = await _shipmentQueryService.GetByOrderIdAsync(orderId);

            return result.ToOk();
        }

        [HttpGet]
        [Route("order_number/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> RetriveShipmentByOrderNumber(string orderNumber)
        {

            var result = await _shipmentQueryService.GetByOrderNumberAsync(orderNumber);

            return result.ToOk();
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ShipmentDto))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> CreateShipment([FromBody] ShipmentModel model)
        {
            var validationResult = await ValidateModel(model);

            if(!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _shipmentCommandService.CreateAsync(model);

            return result.ToCreatedAtAction( nameof(RetriveShipmentById), new {shipmentId = result.Value?.Id});

        }

        [HttpPost]
        [Route("fullfill/{shipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipmentDto))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> FullfillShipment(string shipmentId, [FromBody] PackageModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _shipmentCommandService.FullfillAsync(shipmentId,model);

            return result.ToOk();
        }

        [HttpPost]
        [Route("{shipmentId}/labels")]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> BuyShipmentLabel(string shipmentId, BuyShipmentLabelModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _shipmentCommandService.BuyLabelAsync(shipmentId, model);

            return result.ToOk();

        }

        [HttpPost]
        [Route("{shipmentId}/rates")]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> RetriveShipmentRate(string shipmentId)
        {
            var result  =await  _shipmentCommandService.RetriveShipmentRatesAsync(shipmentId);

            return result.ToOk();
        }


        [HttpPost]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ShipmentListDto>))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> Search([FromBody]ShipmentSearchByOrderNumberModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }
            var result = await _shipmentQueryService.SearchByOrderNumber( model);

            return result.ToOk();

        }


    }
}
