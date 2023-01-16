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

        public Task<HttpResponseResult<Basket>> RetrieveAsync(string userId , CancellationToken  cancellationToken= default)
        {
            return _microStoreClinet.MakeGetRequest<Basket>(string.Format("{0}/{1}", userId), cancellationToken: cancellationToken);
        }


        public Task<HttpResponseResult<Basket>> AddItemAsync(string userId, BasketAddItemRequestOptions options , CancellationToken cancellationToken= default)
        {
            return _microStoreClinet.MakeRequest<BasketAddItemRequestOptions,Basket>(options, string.Format("{0}/{1}", userId), HttpMethod.Post, cancellationToken);
        }


        public Task<HttpResponseResult<Basket>> RemoveItemAsync(string userId, BasketRemoveItemRequestOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<BasketRemoveItemRequestOptions, Basket>(options, string.Format("{0}/{1}", userId), HttpMethod.Delete, cancellationToken);
        }

        public Task<HttpResponseResult<Basket>> MigrateAsync(BasketMigrateRequestOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<BasketMigrateRequestOptions, Basket>(options, BaseUrl + "/" + "migrate", HttpMethod.Put, cancellationToken);
        }
    }
}
