using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.ShoppingCart.Api.Infrastructure;
using MicroStore.ShoppingCart.Api.Models;
using Volo.Abp.Caching;
using Volo.Abp.ObjectMapping;

namespace MicroStore.ShoppingCart.Api.Controllers
{
    [Route("api/baskets")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IDistributedCache<Basket> _distributedCache;

        private readonly IBasketRepository _basketRepository;

        private readonly IObjectMapper _objectMapper;

        public BasketController(IDistributedCache<Basket> distributedCache, IObjectMapper objectMapper, IBasketRepository basketRepository)
        {
            _distributedCache = distributedCache;
            _objectMapper = objectMapper;
            _basketRepository = basketRepository;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<BasketDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError , Type= typeof(Envelope))]
        public async Task<IActionResult> GetUserBasket(string userId)
        {
            var basket = await _basketRepository.GetAsync(userId);

            return Success(StatusCodes.Status200OK, _objectMapper.Map<Basket, BasketDto>(basket));
        }


        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK,Type=  typeof(Envelope<BasketDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> Update(string userId ,BasketModel basket)
        {
            var userBasket = await _basketRepository.GetAsync(userId);

            userBasket.Items = basket.Items.Select(x => new BasketItem { ProductId = x.ProductId, Quantity = x.Quantity }).ToList();

            userBasket =  await _basketRepository.UpdateAsync(userBasket);

            return Success(StatusCodes.Status200OK, _objectMapper.Map<Basket, BasketDto>(userBasket));

        }

        [HttpPost("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<BasketDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> AddProduct(string userId ,  BasketItemModel model)
        {
            var basket = await _basketRepository.GetAsync(userId);

            basket.AddProduct(model.ProductId, model.Quantity);

            basket =  await _basketRepository.UpdateAsync(basket);

            return Success(StatusCodes.Status200OK, _objectMapper.Map<Basket, BasketDto>(basket));

        }

        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<BasketDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RemoveProduct(string userId ,RemoveBasketItemsModel model)
        {
            var basket = await _basketRepository.GetAsync(userId);

            foreach (var productId in model.ProductIds)
            {
                basket.RemoveProduct(productId);
            }

            basket = await _basketRepository.UpdateAsync(basket);

            return Success(StatusCodes.Status200OK, _objectMapper.Map<Basket, BasketDto>(basket));

        }


        [HttpPut("migrate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<BasketDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> Migrate(MigrateDto model)
        {
            var basketFrom = await _basketRepository.GetAsync(model.FromUserId);

            var basketTo = await _basketRepository.GetAsync(model.ToUserId);

            basketTo.Migrate(basketFrom);

            await _basketRepository.RemoveAsync(model.FromUserId);

            await _basketRepository.UpdateAsync(basketTo);


            return Success(StatusCodes.Status202Accepted, _objectMapper.Map<Basket, BasketDto>(basketTo));
        }


        [NonAction]
        protected IActionResult Success<TResult>(int statusCode,TResult result)
        {
            return StatusCode(statusCode, Envelope.Success(result));
        }

        [NonAction]
        protected IActionResult Success(int statusCode)
        {
            return StatusCode(statusCode, Envelope.Success<Unit>(Unit.Value));
        }

        [NonAction]
        protected IActionResult Failure(int statusCode , ErrorInfo error)
        {
            return StatusCode(statusCode, Envelope.Failure<Unit>(error));
        }

    }
}
