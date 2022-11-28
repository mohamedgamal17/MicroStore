using MicroStore.ShoppingCart.Api.Models;

namespace MicroStore.ShoppingCart.Api.Infrastructure
{
    public interface IBasketRepository
    {
         Task<Basket> GetAsync(string userId);
         Task<Basket> UpdateAsync(Basket basket);
         Task RemoveAsync(string userId);
   

    }
}
