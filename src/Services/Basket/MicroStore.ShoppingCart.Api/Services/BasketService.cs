using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.ShoppingCart.Api.Infrastructure;
using MicroStore.ShoppingCart.Api.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.ShoppingCart.Api.Services
{
    public class BasketService : ApplicationService, IBasketService
    {

        private readonly IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Result<BasketDto>> GetAsync(string userId)
        {
            var basket = await _basketRepository.GetAsync(userId);

            return ObjectMapper.Map<Basket, BasketDto>(basket);
        }

        public async Task<Result<BasketDto>> AddOrUpdate(string userId, BasketModel model)
        {
            var userBasket = await _basketRepository.GetAsync(userId);

            userBasket.Items = model.Items.Select(x => new BasketItem { ProductId = x.ProductId, Quantity = x.Quantity }).ToList();

            userBasket = await _basketRepository.UpdateAsync(userBasket);

            return ObjectMapper.Map<Basket, BasketDto>(userBasket);
        }

        public async Task<Result<BasketDto>> AddOrUpdateProduct(string userId, BasketItemModel model)
        {

            var basket = await _basketRepository.GetAsync(userId);

            basket.AddProduct(model.ProductId, model.Quantity);

            basket = await _basketRepository.UpdateAsync(basket);

            return ObjectMapper.Map<Basket, BasketDto>(basket);
        }


        public async Task<Result<BasketDto>> RemoveProduct(string id, RemoveBasketItemModel model)
        {
            var basket = await _basketRepository.GetAsync(id);

            var result = basket.RemoveProduct(model.ProductId, model.Quantity);

            if (result.IsFailure)
            {
                return new Result<BasketDto>(result.Exception);
            }

            basket = await _basketRepository.UpdateAsync(basket);

            return ObjectMapper.Map<Basket, BasketDto>(basket);
        }

        public async Task<Result<BasketDto>> MigrateAsync(MigrateModel model)
        {

            var basketFrom = await _basketRepository.GetAsync(model.FromUserId);

            var basketTo = await _basketRepository.GetAsync(model.ToUserId);

            basketTo.Migrate(basketFrom);

            await _basketRepository.RemoveAsync(model.FromUserId);

            await _basketRepository.UpdateAsync(basketTo);

            return ObjectMapper.Map<Basket, BasketDto>(basketTo);
        }

        public async Task<Result<Unit>> Clear(string id)
        {
            await _basketRepository.RemoveAsync(id);

            return Unit.Value;
        }
    }
}
