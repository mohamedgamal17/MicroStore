using MicroStore.ShoppingCart.Api.Models;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace MicroStore.ShoppingCart.Api.Infrastructure
{
    public class BasketRepository : IBasketRepository , ITransientDependency
    {
        private readonly IDistributedCache<Basket, string> _cache;

        public BasketRepository(IDistributedCache<Basket, string> cache)
        {
            _cache = cache;
        }

        public Task<Basket> GetAsync(string userId)
        {
            return _cache.GetOrAddAsync(userId, () => Task.FromResult(new Basket
            {
                UserId = userId
            }));
        }


        public async Task<Basket> UpdateAsync(Basket basket)
        {
            await  _cache.SetAsync(basket.UserId, basket);

            return basket;
        }


        public  Task RemoveAsync(string userId)
        {
            return _cache.RemoveAsync(userId);
        }



    }
}
