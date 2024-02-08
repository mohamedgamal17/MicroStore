using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Ordering;
using MicroStore.Bff.Shopping.Infrastructure;
using MicroStore.Bff.Shopping.Services.Ordering;

namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/user/orders")]
    [ApiController]
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly OrderService _orderService;

        private readonly IWorkContext _workContext;
        public UserOrderController(OrderService orderService, IWorkContext workContext)
        {
            _orderService = orderService;
            _workContext = workContext;
        }


        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<Order>))]
        public async Task<ActionResult<Order>> ListOrders(string? orderNumber = null, string? states = null, DateTime? startDate = null, DateTime? endDate = null, int skip = 0, int length = 10, string? sortBy = null, bool desc = false)
        {
            string userId = _workContext.User!.Id!;

            var result = await _orderService.ListAsync(userId, orderNumber, states, startDate, endDate, skip, length, sortBy, desc);

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
