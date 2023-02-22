using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.ShippingSystems;
using System.Net;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/systems")]
    public class ShipmentSystemController : MicroStoreApiController
    {
        private readonly IShippingSystemCommandService _shippingSystemCommandService;

        private readonly IShippingSystemQueryService _shippingSystemQueryService;

        public ShipmentSystemController(IShippingSystemCommandService shippingSystemCommandService, IShippingSystemQueryService shippingSystemQueryService)
        {
            _shippingSystemCommandService = shippingSystemCommandService;
            _shippingSystemQueryService = shippingSystemQueryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(List<ShippingSystemDto>))]
   
        public async Task<IActionResult> RetriveShipmentSystemList()
        {

            var result = await _shippingSystemQueryService.ListAsync();

            return FromResultV2(result,HttpStatusCode.OK);           
        }

        [HttpGet]
        [Route("system_name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShippingSystemDto))]
        public async Task<IActionResult> RetriveShipmentSystemWithName(string name)
        {
            var result = await _shippingSystemQueryService.GetByNameAsync(name);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("{systemId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShippingSystemDto))]

        public async Task<IActionResult> RetriveShipmentSystem(string systemId)
        {
            var result = await _shippingSystemQueryService.GetAsync(systemId);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpPut]
        [Route("{systemName}")]
        [ProducesResponseType(StatusCodes.Status202Accepted,Type = typeof(ShippingSystemDto))]
        public async Task<IActionResult> UpdateShipmentSystem(string systemName, [FromBody] UpdateShippingSystemModel model)
        {
            var result = await _shippingSystemCommandService.EnableAsync(systemName,model.IsEnabled);

            return FromResultV2(result, HttpStatusCode.OK);
        }
    }

    public class UpdateShippingSystemModel
    {
        public bool IsEnabled { get; set; } 
    }
}
