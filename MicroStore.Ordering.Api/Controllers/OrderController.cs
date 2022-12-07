using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Ordering.Application.Abstractions.Interfaces;
using Volo.Abp;

namespace MicroStore.Ordering.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [RemoteService(Name = "Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IPublishEndpoint _publishEndPoint;

        public OrderController(IPublishEndpoint publishEndPoint)
        {
            _publishEndPoint = publishEndPoint;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }


        [HttpPost("submit")]
        public async Task<IActionResult> SubmitOrder()
        {
            throw new NotImplementedException();
        }



        [HttpPost("fullfill/{orderId}")]
        public Task<IActionResult> FullfillOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }


        [HttpPost("complete/{orderId}")]
        public async Task<IActionResult> CompleteOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }



        [HttpPost("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }


    }
}
