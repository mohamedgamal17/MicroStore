using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Inventory;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Inventory
{
    public class OrderService
    {
        const string BaseUrl = "inventory/orders";


        private readonly MicroStoreClinet _microStoreClinet;

        public OrderService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }


        public async Task<HttpResponseResult<PagedList<OrderList>>> ListAsync(PagingReqeustOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PagedList<OrderList>>(BaseUrl, HttpMethod.Get, options, cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<OrderList>>> ListByUserAsync(string userId,PagingReqeustOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<PagedList<OrderList>>(string.Format("{0}/user/{1}", BaseUrl, userId), HttpMethod.Get, options, cancellationToken);
        }

        public async Task<HttpResponseResult<Order>> RetrieveAsync(Guid orderId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Order>(string.Format("{0}/{1}", BaseUrl, orderId), HttpMethod.Get,  cancellationToken);
        }

        public async Task<HttpResponseResult<Order>> RetrieveByExternalIdAsync(Guid externalOrderId, CancellationToken cancellationToken)
        {
            return await _microStoreClinet.MakeRequest<Order>(string.Format("{0}/external_order_id/{1}", BaseUrl, externalOrderId),HttpMethod.Get , cancellationToken);
        }


    }
}
