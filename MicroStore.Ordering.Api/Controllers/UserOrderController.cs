using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Orders;
using MicroStore.Ordering.Application.Security;
using System.Net;

namespace MicroStore.Ordering.Api.Controllers
{
    [ApiController]

    [Route("api/user/orders")]
    public class UserOrderController : MicroStoreApiController
    {
        private readonly IOrderCommandService _orderCommandService;

        private readonly IOrderQueryService _orderQueryService;
        private readonly ILogger<UserOrderController> _logger;

        public UserOrderController(IOrderCommandService orderCommandService, IOrderQueryService orderQueryService, ILogger<UserOrderController> logger)
        {
            _orderCommandService = orderCommandService;
            _orderQueryService = orderQueryService;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy =ApplicationSecurityPolicies.RequireOrderWriteScope)]

        public async Task<IActionResult> SubmitOrder(OrderModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var orderModel = new CreateOrderModel
            {
                TaxCost = model.TaxCost,
                ShippingCost = model.ShippingCost,
                SubTotal = model.SubTotal,
                TotalPrice = model.TotalPrice,
                ShippingAddress = model.ShippingAddress,
                BillingAddress = model.BillingAddress,
                Items = model.Items,
                UserId = CurrentUser.UserId!,
            };

            var result = await _orderCommandService.CreateOrderAsync(orderModel);


            return result.ToAcceptedAtAction(nameof(RetirveUserOrder),new {id = result.Value?.Id} );
        }


        [HttpGet]
        [Route("")]
        [ActionName(nameof(RetirveUserOrderList))]
        [Authorize(Policy = ApplicationSecurityPolicies.RequireOrderReadScope)]
        public async Task<IActionResult> RetirveUserOrderList([FromQuery] OrderListQueryModel queryParams)
        {
            var userClaims = HttpContext.User.Claims;

            var userId = CurrentUser.UserId!;

            _logger.LogInformation("User claism : {userClaims}", userClaims);

            _logger.LogInformation("User Id {userId}", userId);

            var result = await _orderQueryService.ListAsync(queryParams, CurrentUser.UserId!);

            return result.ToOk();
        }


        [HttpGet]
        [Route("{orderId}")]
        [ActionName(nameof(RetirveUserOrder))]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [Authorize(Policy = ApplicationSecurityPolicies.RequireOrderReadScope)]
        public async Task<IActionResult> RetirveUserOrder(Guid orderId)
        {

            var result = await _orderQueryService.GetAsync(orderId);

            return result.ToOk();
        }

        [HttpGet]
        [Route("order_number/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [Authorize(Policy = ApplicationSecurityPolicies.RequireOrderReadScope)]
        public async Task<IActionResult> RetirveOrderByNumber(string orderNumber)
        {

            var result = await _orderQueryService.GetByOrderNumberAsync(orderNumber);

            return result.ToOk();
        }
    }
}
