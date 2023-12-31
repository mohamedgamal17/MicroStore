﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.ShippingSystems;
using MicroStore.Shipping.Domain.Security;
namespace MicroStore.Shipping.Host.Controllers
{
    [ApiController]
    [Route("api/systems")]
    [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
    public class ShipmentSystemController : MicroStoreApiController
    {

        private readonly IShippingSystemQueryService _shippingSystemQueryService;

        public ShipmentSystemController(IShippingSystemQueryService shippingSystemQueryService)
        {
            _shippingSystemQueryService = shippingSystemQueryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ShippingSystemDto>))]

        public async Task<IActionResult> RetriveShipmentSystemList()
        {

            var result = await _shippingSystemQueryService.ListAsync();

            return result.ToOk();
        }

    }
}
