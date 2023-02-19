using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Security;
using MicroStore.Ordering.Api.Models;
using MicroStore.Ordering.Application.Orders;
using MicroStore.Ordering.Application.Security;

namespace MicroStore.Ordering.Api.Controllers
{
    [ApiController]
    [Route("api/user/orders")]
    public class UserOrderController : MicroStoreApiController
    {

        private readonly IApplicationCurrentUser _currentUser;

        public UserOrderController(IApplicationCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpPost]
        [Route("")]
  //      [RequiredScope(OrderingScope.Order.Submit)]

        public async Task<IActionResult> SubmitOrder(OrderModel model)
        {
            var command = ObjectMapper.Map<OrderModel, SubmitOrderCommand>(model);


            command.UserId = _currentUser.Id;

            var result = await Send(command);

            return FromResult(result);
        }


        [HttpGet]
        [Route("")]
    //    [RequiredScope(OrderingScope.Order.List)]
        public async Task<IActionResult> RetirveUserOrderList( [FromQuery] PagingAndSortingParamsQueryString @params)
        {          

            var query = new GetUserOrderListQuery
            {
                UserId = _currentUser.Id,
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await Send(query);

            return FromResult(result);
        }


        [HttpGet]
        [Route("{orderId}")]
  //      [RequiredScope(OrderingScope.Order.Read)]
        public async Task<IActionResult> RetirveUserOrder(Guid orderId)
        {
            var query = new GetOrderQuery
            {
                OrderId = orderId,
            };

            var result = await Send(query);

            return FromResult(result);
        }
    }
}
