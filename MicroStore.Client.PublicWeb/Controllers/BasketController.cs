﻿using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.ShoppingGateway.ClinetSdk;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
namespace MicroStore.Client.PublicWeb.Controllers
{

    [Route("api/basket")]
    public class BasketController : Controller
    {

        private readonly IWorkContext _workContext;

        private readonly BasketService _basketService;

        private readonly BasketAggregateService _basketAggregateService;

        public BasketController(IWorkContext workContext, BasketService basketService, BasketAggregateService basketAggregateService)
        {
            _workContext = workContext;
            _basketService = basketService;
            _basketAggregateService = basketAggregateService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> RetriveBasket()
        {
            var basket =  await _basketAggregateService
                .RetriveBasket(_workContext.TryToGetCurrentUserId());

            return Ok(basket);
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddBasketItem(BasketItemModel model)
        {
            var options = new BaskeItemRequestOptions
            {
                ProductId = model.ProductId.ToString(),
                Quantity = model.Quantity,

            };

            var basketResponse = await _basketService.AddItemAsync(_workContext.TryToGetCurrentUserId(),options);

            
            return FromHttpResponseResult(basketResponse);
        }


        private IActionResult FromHttpResponseResult<T>(HttpResponseResult<T> response)
        {
            return StatusCode((int)response.HttpStatusCode, response.HttpEnvelopeResult);
        }


        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateBasket(BasketModel model)
        {
            var options = new BasketRequestOptions()
            {
                Items = model.Items.Select(x => new BaskeItemRequestOptions { ProductId = x.ProductId.ToString(), Quantity = x.Quantity }).ToList()
            };

            var basketResponse = await _basketService.UpdateAsync(_workContext.TryToGetCurrentUserId(), options);

            return FromHttpResponseResult(basketResponse);
        }


        [HttpDelete]
        [Route("")]
     
        public async Task<IActionResult> RemoveBasketItem(RemoveBasketItemModel model)
        {
            var options = new BasketRemoveItemRequestOptions
            {
                ProductIds = new string[] { model.ProductId.ToString() }
            };

            var basketResponse = await _basketService.RemoveItemsAsync(_workContext.TryToGetCurrentUserId(), options);


            return FromHttpResponseResult(basketResponse);
        }
    }


    public class BasketItemModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }


    public class BasketModel
    {
        public List<BasketItemModel> Items { get; set; }

    }

    public class RemoveBasketItemModel
    {
        public Guid ProductId { get; set; }
    }
}