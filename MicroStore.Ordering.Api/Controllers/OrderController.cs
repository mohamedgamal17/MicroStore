using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using System.Net;
using Volo.Abp;

namespace MicroStore.Ordering.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [RemoteService(Name = "Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {


        private readonly ILocalMessageBus _localMessageBus;

        public OrderController(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }


        [HttpPut("pay-order/{orderId}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public  Task<PaymentDto> PayOrder(Guid orderId)
        {
            var command = new PayOrderCommand { OrderId = orderId };

            return _localMessageBus.Send(command); 
        }


        [HttpPut("confirm-order/{orderId}")]
        public async Task<IActionResult> ConfirmOrder(Guid orderId)
        {

            var command = new ConfirmOrderCommand { OrderId = orderId };

            await _localMessageBus.Send(command);

            return Accepted();

        }
    }
}
