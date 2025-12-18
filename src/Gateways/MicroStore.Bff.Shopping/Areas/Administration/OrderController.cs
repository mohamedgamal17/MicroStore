using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Ordering;
using MicroStore.Bff.Shopping.Services.Ordering;

namespace MicroStore.Bff.Shopping.Areas.Administration
{
    [ApiExplorerSettings(GroupName = "Administration")]
    [Route("api/administration/orders")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(PagedList<Order>))]
        public async Task<ActionResult<PagedList<Order>>> ListOrdersAsync(string userId = "", string orderNumber = "", string states = "", DateTime startDate = default, DateTime endDate = default, int skip = 0, int length = 10, string sortBy = "", bool desc = false)
        {
            var result = await _orderService
                .ListAsync(userId, orderNumber, states, startDate, endDate, skip, length, sortBy,desc);

            return Ok(result);
        }

        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        public async Task<ActionResult<Order>> GetByIdAsync(string orderId)
        {
            var result = await _orderService.GetAsync(orderId);

            return Ok(result);
        }

        [HttpGet]
        [Route("number/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        public async Task<ActionResult<Order>> GetByOrderNumber(string orderNumber)
        {
            var result = await _orderService.GetByOrderNumberAsync(orderNumber);

            return Ok(result);
        }
    }
}
