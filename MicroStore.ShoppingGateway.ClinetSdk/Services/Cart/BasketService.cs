using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Cart
{
    public class BasketService
    {

        const string BaseUrl = "/shoppingcart/baskets";

        private readonly MicroStoreClinet _microStoreClinet;

        public BasketService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<Basket> RetrieveAsync(string userId , CancellationToken  cancellationToken= default)
        {
            return await _microStoreClinet.MakeRequest<Basket>(string.Format("{0}/{1}",BaseUrl ,userId), HttpMethod.Get, cancellationToken: cancellationToken);
        }


        public async Task<Basket> AddItemAsync(string userId, BaskeItemRequestOptions options , CancellationToken cancellationToken= default)
        {
            return await _microStoreClinet.MakeRequest<Basket>(string.Format("{0}/{1}", BaseUrl, userId), HttpMethod.Post, options, cancellationToken);
        }

        public async Task<Basket> UpdateAsync(string userId , BasketRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Basket>(string.Format("{0}/{1}", BaseUrl, userId), HttpMethod.Put, options, cancellationToken);
        }


        public async Task<Basket> RemoveItemsAsync(string userId , BasketRemoveItemRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Basket>(string.Format("{0}/{1}", BaseUrl, userId), HttpMethod.Delete, options, cancellationToken);
        }

        public async Task<Basket> MigrateAsync(BasketMigrateRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Basket>(BaseUrl + "/" + "migrate", HttpMethod.Put, options, cancellationToken);
        }
    }
}
