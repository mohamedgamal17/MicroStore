using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Payment.Application.PaymentSystems;
using MicroStore.Payment.Domain.Shared.Dtos;
namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/systems")]
    public class SystemController : MicroStoreApiController
    {
        private readonly IPaymentSystemCommandService _paymentSystemCommandService;

        private readonly IPaymentSystemQueryService _paymentSystemQueryService;

        public SystemController(IPaymentSystemCommandService paymentSystemCommandService, IPaymentSystemQueryService paymentSystemQueryService)
        {
            _paymentSystemCommandService = paymentSystemCommandService;
            _paymentSystemQueryService = paymentSystemQueryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(PaymentSystemDto))]
        public async Task<IActionResult> RetirveSystems()
        {
            var result = await _paymentSystemQueryService.ListPaymentSystemAsync();

            return result.ToOk();
        }

        [HttpGet]
        [Route("system_name/{systemName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentSystemDto))]
        public async Task<IActionResult> RetirveSystemWithName(string systemName)
        {
            var result = await _paymentSystemQueryService.GetBySystemNameAsync(systemName);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{systemId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentSystemDto))]
        public async Task<IActionResult> RetirveSystems(string systemId)
        {
            var result = await _paymentSystemQueryService.GetAsync(systemId);

            return result.ToOk();
        }


        [HttpPut]
        [Route("{systemName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentSystemDto))]
        public async Task<IActionResult> UpdatePluginSystem(string systemName,[FromBody] UpdatePluginSystemModel model)
        {
            var result = await _paymentSystemCommandService.EnablePaymentSystemAsync(systemName, model.IsEnabled);

            return result.ToOk();
        }

    }


    public class UpdatePluginSystemModel
    {
        public bool IsEnabled { get; set; }
    }
}
