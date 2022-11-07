using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.ShoppingCart.Api.Models;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Application.Abstraction.Queries;
using System.Net;

namespace MicroStore.ShoppingCart.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly ILocalMessageBus _localMessageBus;

        public BasketController(ILocalMessageBus localMessageBus)
        {

            _localMessageBus = localMessageBus;
        }


        [Route("")]
        [HttpGet]

        public async Task<IActionResult> Get()
        {

            GetBasketQuery query = new GetBasketQuery();


            var result = await _localMessageBus.Send(query);

            return Ok(result);

        }

        [Route("")]
        [HttpPost]
        public async Task<BasketDto> CreateBasket()
        {
            CreateBasketCommand createBasketCommand = new CreateBasketCommand();

            var result = await _localMessageBus.Send(createBasketCommand);

            return result;
        }


        [Route("{basketId}/AddBasketItem")]
        [HttpPost]
        public async Task<IActionResult> Post(Guid basketId,[FromBody] CreateBasketItemModel model)
        {

            AddBasketItemCommand command = new AddBasketItemCommand
            {
                BasketId = basketId,
                ProductId = model.ProductId,
                Quantity = model.Quantity
            };

            var result = await _localMessageBus.Send(command);

            return Ok(result);
        }


        [Route("{basketId}/UpdateBasketItem/{basketItemId}")]
        [HttpPut]
        public async Task<IActionResult> Put(Guid basketId,Guid basketItemId, [FromBody] UpdateBasketItemModel model)
        {

            UpdateBasketItemQuantityCommand command = new UpdateBasketItemQuantityCommand
            {
                BasketId = basketId,
                BasketItemId = basketItemId,
                Quantity = model.Quantity
            };

            var result = await _localMessageBus.Send(command);

            return Accepted(result);
        }


        [Route("RemoveBasketItem/{basketItemId}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid basketItemId)
        {
            RemoveBasketItemCommand command = new RemoveBasketItemCommand
            {
                BasketItemId = basketItemId
            };

            await _localMessageBus.Send(command);

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [Route("Checkout")]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {


            CheckoutCommand command = new CheckoutCommand
            {
                ShippingAddressId = model.ShippingAddressId,
                BillingAddressId = model.BillingAddressId,

            };

            var result = await _localMessageBus.Send(command);

            return Accepted(result);
        }

    }
}
