using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Payment.Application.PaymentSystems;
using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Api.Host.Controllers
{
    [ApiController]
    [Route("api/systems")]
    public class SystemController : MicroStoreApiController
    {
        private readonly IPaymentSystemQueryService _paymentSystemQueryService;

        public SystemController(IPaymentSystemQueryService paymentSystemQueryService)
        {
            _paymentSystemQueryService = paymentSystemQueryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentSystemDto))]
        public async Task<IActionResult> RetirveSystems()
        {
            var result = await _paymentSystemQueryService.ListAsync();

            return result.ToOk();
        }

    }
}
