using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingCart.Api.Infrastructure;
using MicroStore.ShoppingCart.Api.Models;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.ObjectMapping;

namespace MicroStore.ShoppingCart.Api.Controllers
{

    [RemoteService(Name = "Basket")]
    [Route("api/[controller]")]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<IActionResult> GetUserBasket(string userId)
        {
            var basket = await _basketRepository.GetAsync(userId);

            return Ok(_objectMapper.Map<Basket, BasketDto>(basket));
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status202Accepted,Type=  typeof(BasketDto))]
        public async Task<IActionResult> Update(Basket basket)
        {
            basket =  await _basketRepository.UpdateAsync(basket);

            return Accepted(_objectMapper.Map<Basket, BasketDto>(basket));
        }

        [HttpPost("add-item")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(BasketDto))]
        public async Task<IActionResult> AddProduct(AddProductDto model)
        {
            var basket = await _basketRepository.GetAsync(model.UserId);

            basket.AddProduct(model.ProductId, model.Quantity);

            basket =  await _basketRepository.UpdateAsync(basket);

            return Ok(_objectMapper.Map<Basket, BasketDto>(basket));
        }

        [HttpDelete("remove-item")]
        [ProducesResponseType(StatusCodes.Status202Accepted,Type = typeof(BasketDto))]
        public async Task<IActionResult> RemoveProduct(RemoveProductDto model)
        {
            var basket = await _basketRepository.GetAsync(model.UserId);

            var result =  basket.RemoveProduct(model.ProductId);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            basket = await _basketRepository.UpdateAsync(basket);

            return Accepted(_objectMapper.Map<Basket, BasketDto>(basket));
        }


        [HttpPut("migrate")]
        [ProducesResponseType(StatusCodes.Status202Accepted,Type =  typeof(BasketDto))]
        public async Task<IActionResult> Migrate(MigrateDto model)
        {
            var basketFrom = await _basketRepository.GetAsync(model.FromUserId);

            var basketTo = await _basketRepository.GetAsync(model.ToUserId);

            basketTo.Migrate(basketFrom);

            await _basketRepository.RemoveAsync(model.FromUserId);

            await _basketRepository.UpdateAsync(basketTo);

            return Accepted(_objectMapper.Map<Basket, BasketDto>(basketTo));

        }



    }
}
