using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.ShoppingCart.Api.Models;

namespace MicroStore.ShoppingCart.Api.Services
{
    public interface IBasketService
    {
        Task<Result<BasketDto>> AddOrUpdate(string userId, BasketModel model);
        Task<Result<BasketDto>> AddOrUpdateProduct(string userId, BasketItemModel model);
        Task<Result<Unit>> Clear(string id);
        Task<Result<BasketDto>> GetAsync(string userId);
        Task<Result<BasketDto>> MigrateAsync(MigrateModel model);
        Task<Result<BasketDto>> RemoveProduct(string id, RemoveBasketItemModel model);
    }
}