using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.ShoppingCart.Api.Models;
using MicroStore.ShoppingCart.Api.Services;
namespace MicroStore.ShoppingCart.Api.Controllers
{
    [Route("api/baskets")]
    [ApiController]
    //  [Authorize]
    public class BasketController : MicroStoreApiController
    {
        private readonly IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        public async Task<IActionResult> GetUserBasket(string id)
        {
            var result = await _basketService.GetAsync(id);

            return result.ToOk();
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

            var result = await _basketService.AddOrUpdate(id, model);
       
            return result.ToOk();

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

            var result = await _basketService.AddOrUpdateProduct(id, model);

            return result.ToOk();
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

            var result = await _basketService.RemoveProduct(id, model);

            return result.ToOk();
        }


        [HttpPut("migrate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Migrate(MigrateModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _basketService.MigrateAsync(model);

            return result.ToOk();
        }


        [Route("clear/{id}")]
        [HttpPost]
        public async Task<IActionResult> Clear(string id)
        {
            var result = await _basketService.Clear(id);

            return result.ToNoContent();
        }

    }
}
