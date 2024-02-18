using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.ShoppingCart;
using MicroStore.Bff.Shopping.Infrastructure;
using MicroStore.Bff.Shopping.Models.ShoppingCart;
using MicroStore.Bff.Shopping.Services.ShoppingCart;

namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/basket")]
    [ApiController]
    public class BasketController : Controller
    {
        private readonly BasketService _basketService;
        private readonly IWorkContext _workContext;

        public BasketController(BasketService basketService, IWorkContext workContext)
        {
            _basketService = basketService;
            _workContext = workContext;
        }

        [Route("{userId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Basket))]
        public async Task<IActionResult> GetUserBasket()
        {
            string userId = _workContext.User!.Id!;

            var basket = await _basketService.GetAsync(userId);

            return Ok(basket);
        }


        [Route("items")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Basket))]
        public async Task<IActionResult> AddOrUpdateItem(BasketItemModel model)
        {
            string userId = _workContext.User!.Id!;

            var basket = await _basketService.AddOrUpdateItemAsync(userId, model);

            return Ok(basket);
        }

        [Route("items")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Basket))]
        public async Task<IActionResult> RemoveItem(RemoveBasketItemModel model) 
        {
            string userId = _workContext.User!.Id!;

            var basket = await _basketService.RemoveItemAsync(userId, model);

            return Ok(basket);
        }

        [Route("clear")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Basket))]
        public async Task<IActionResult> Clear()
        {
            string userId = _workContext.User!.Id!;

            await _basketService.Clear(userId);

            return NoContent();
        }

        [Route("migrate")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Basket))]
        public async Task<IActionResult> Migrate(BasketMigrationModel model)
        {
            var basket = await _basketService.MigrateAsync(model);

            return Ok(basket);
        }

        [Route("Checkout")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Basket))]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            string userId = _workContext.User!.Id!;

            var result = await _basketService.Checkout(userId, model);

            return Ok(result);
        }

    }
}
