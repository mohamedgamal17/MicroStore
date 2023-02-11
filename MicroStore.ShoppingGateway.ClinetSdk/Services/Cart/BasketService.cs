using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Cart
{
    public class BasketService
    {

        const string BaseUrl = "/baskets";

        private readonly MicroStoreClinet _microStoreClinet;

        public BasketService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<HttpResponseResult<Basket>> RetrieveAsync(string userId , CancellationToken  cancellationToken= default)
        {
            return await _microStoreClinet.MakeRequest<Basket>(string.Format("{0}/{1}", userId), HttpMethod.Get, cancellationToken: cancellationToken);
        }


        public async Task<HttpResponseResult<Basket>> AddItemAsync(string userId, BasketAddItemRequestOptions options , CancellationToken cancellationToken= default)
        {
            return await _microStoreClinet.MakeRequest<Basket>(string.Format("{0}/{1}", userId), HttpMethod.Post, options, cancellationToken);
        }


        public async Task<HttpResponseResult<Basket>> RemoveItemAsync(string userId, BasketRemoveItemRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Basket>(string.Format("{0}/{1}", userId), HttpMethod.Delete, options, cancellationToken);
        }

        public async Task<HttpResponseResult<Basket>> MigrateAsync(BasketMigrateRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Basket>(BaseUrl + "/" + "migrate", HttpMethod.Put, options, cancellationToken);
        }
    }
}
