using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Orders;
namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : MicroStoreApiController
    {
        private readonly IOrderCommandService _orderCommandService;

        private readonly IOrderQueryService _orderQueryService;
        public OrderController(IOrderCommandService orderCommandService, IOrderQueryService orderQueryService)
        {
            _orderCommandService = orderCommandService;
            _orderQueryService = orderQueryService;
        }



        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(OrderListDto))]
        public async Task<IActionResult> RetirveOrderList([FromQuery]PagingAndSortingParamsQueryString @params ,[FromQuery(Name ="user_id")] string? userId= null)
        {
            var queryParams = new PagingAndSortingQueryParams
            {
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                Lenght = @params.Lenght,
                Skip = @params.Skip,
            };

            var result = await _orderQueryService.ListAsync(queryParams, userId);

            return result.ToOk();
        }

      

        [HttpGet]
        [Route("{orderId}")]
        [ActionName(nameof(RetirveOrder))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]

        public async Task<IActionResult> RetirveOrder(Guid orderId)
        {

            var result = await _orderQueryService.GetAsync(orderId);

            return result.ToOk();
        }

        [HttpGet]
        [Route("order_number/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<IActionResult> RetirveOrderByNumber(string orderNumber)
        {

            var result = await _orderQueryService.GetByOrderNumberAsync(orderNumber);

            return result.ToOk();
        }


        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]

        public async Task<IActionResult> SubmitOrder([FromBody]CreateOrderModel model)
        {

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return InvalideModelState();
            }

            var result  = await _orderCommandService.CreateOrderAsync(model);

            return result.ToAcceptedAtAction(nameof(RetirveOrder), new {id = result.Value?.Id});
        }



        [HttpPost("fullfill/{orderId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]

        public async Task<IActionResult> FullfillOrder(Guid orderId,[FromBody] FullfillOrderModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _orderCommandService.FullfillOrderAsync(orderId,model);

            return result.ToAcceptedAtAction(nameof(RetirveOrder), new { id = orderId });
        }


        [HttpPost("complete/{orderId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> CompleteOrder(Guid orderId)
        {
            
            var result = await _orderCommandService.CompleteOrderAsync(orderId);

            return result.ToAcceptedAtAction(nameof(RetirveOrder), new {id = orderId});
        }



        [HttpPost("cancel/{orderId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> CancelOrder(Guid orderId, [FromBody] CancelOrderModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _orderCommandService.CancelOrderAsync(orderId,model);

            return result.ToAcceptedAtAction(nameof(RetirveOrder), new { id = orderId });

        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<OrderDto>))]
        public async Task<IActionResult> Search([FromBody] OrderSearchModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _orderQueryService.SearchByOrderNumber(model);

            return result.ToOk();
        }

    }
}
