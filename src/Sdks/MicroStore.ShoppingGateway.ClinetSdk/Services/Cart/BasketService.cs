using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Cart
{
    public class BasketService : Service
    {

        const string BaseUrl = "/shoppingcart/baskets";

        public BasketService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<Basket> RetrieveAsync(string userId ,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken  cancellationToken= default)
        {
            return await MakeRequestAsync<Basket>(string.Format("{0}/{1}",BaseUrl ,userId), HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }


        public async Task<Basket> AddItemAsync(string userId, BaskeItemRequestOptions options , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken= default)
        {
            return await MakeRequestAsync<Basket>(string.Format("{0}/{1}", BaseUrl, userId), HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<Basket> UpdateAsync(string userId , BasketRequestOptions options ,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Basket>(string.Format("{0}/{1}", BaseUrl, userId), HttpMethod.Put, options, requestHeaderOptions,cancellationToken);
        }


        public async Task<Basket> RemoveItemsAsync(string userId , BasketRemoveItemRequestOptions options,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Basket>(string.Format("{0}/{1}", BaseUrl, userId), HttpMethod.Delete, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<Basket> MigrateAsync(BasketMigrateRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Basket>(BaseUrl + "/" + "migrate", HttpMethod.Put, options, requestHeaderOptions, cancellationToken);
        }

        public async Task ClearAsync(string userId , RequestHeaderOptions requestHeaderOptions = null , CancellationToken cancellationToken = default)
        {
            await MakeRequestAsync(string.Format("{0}/clear/{1}", BaseUrl, userId), HttpMethod.Post, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }
    }
}
