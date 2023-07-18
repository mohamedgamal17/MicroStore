using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.ShoppingCart.Api.Infrastructure;
using MicroStore.ShoppingCart.Api.Models;
using Volo.Abp.Caching;
using Volo.Abp.ObjectMapping;

namespace MicroStore.ShoppingCart.Api.Controllers
{
    [Route("api/baskets")]
    [ApiController]
    //  [Authorize]
    public class BasketController : MicroStoreApiController
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<IActionResult> GetUserBasket(string id)
        {
            var basket = await _basketRepository.GetAsync(id);

            return Success(StatusCodes.Status200OK, _objectMapper.Map<Basket, BasketDto>(basket));
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(string id, BasketModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var userBasket = await _basketRepository.GetAsync(id);

            userBasket.Items = model.Items.Select(x => new BasketItem { ProductId = x.ProductId, Quantity = x.Quantity }).ToList();

            userBasket = await _basketRepository.UpdateAsync(userBasket);

            return Success(StatusCodes.Status200OK, _objectMapper.Map<Basket, BasketDto>(userBasket));

        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddProduct(string id, BasketItemModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var basket = await _basketRepository.GetAsync(id);

            basket.AddProduct(model.ProductId, model.Quantity);

            basket = await _basketRepository.UpdateAsync(basket);

            return Success(StatusCodes.Status200OK, _objectMapper.Map<Basket, BasketDto>(basket));

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveProduct(string id, RemoveBasketItemModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var basket = await _basketRepository.GetAsync(id);

            var result = basket.RemoveProduct(model.ProductId, model.Quantity);

            if (result.IsFailure)
            {
                return result.ToFailure();
            }

            basket = await _basketRepository.UpdateAsync(basket);

            return Success(StatusCodes.Status200OK, _objectMapper.Map<Basket, BasketDto>(basket));

        }


        [HttpPut("migrate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Migrate(MigrateDto model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var basketFrom = await _basketRepository.GetAsync(model.FromUserId);

            var basketTo = await _basketRepository.GetAsync(model.ToUserId);

            basketTo.Migrate(basketFrom);

            await _basketRepository.RemoveAsync(model.FromUserId);

            await _basketRepository.UpdateAsync(basketTo);


            return Success(StatusCodes.Status202Accepted, _objectMapper.Map<Basket, BasketDto>(basketTo));
        }


        [NonAction]
        protected IActionResult Success<TResult>(int statusCode, TResult result)
        {
            return StatusCode(statusCode, result);
        }

        [NonAction]
        protected IActionResult Success(int statusCode)
        {
            return StatusCode(statusCode);
        }


    }
}
