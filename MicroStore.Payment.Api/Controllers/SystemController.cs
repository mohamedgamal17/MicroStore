using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Api.Models.Systems;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using MicroStore.Payment.Domain.Security;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/systems")]
    public class SystemController : MicroStoreApiController
    {
        [HttpGet]
        [Route("")]
        [RequiredScope(BillingScope.System.List)]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(Envelope<ListResultDto<PaymentSystemDto>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]

        public async Task<IActionResult> RetirveSystems()
        {
            var query = new GetPaymentRequestListQuery();

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("system_name/{systemName}")]
        [RequiredScope(BillingScope.System.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ListResultDto<PaymentSystemDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]

        public async Task<IActionResult> RetirveSystemWithName(string systemName)
        {
            var query = new GetPaymentSystemWithNameQuery()
            {
                SystemName = systemName
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{systemId}")]
        [RequiredScope(BillingScope.System.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ListResultDto<PaymentSystemDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]

        public async Task<IActionResult> RetirveSystems(Guid systemId)
        {
            var query = new GetPaymentSystemQuery()
            {
                SystemId = systemId
            };

            var result = await Send(query);

            return FromResult(result);
        }


        [HttpPut]
        [Route("{systemName}")]
        [RequiredScope(BillingScope.System.Update)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> UpdatePluginSystem(string systemName,[FromBody] UpdatePluginSystemModel model)
        {
            var command = new UpdatePaymentSystemCommand
            {
                Name = systemName,
                IsEnabled = model.IsEnabled,
            };

            var result = await Send(command);

            return FromResult(result);
        }

    }
}
