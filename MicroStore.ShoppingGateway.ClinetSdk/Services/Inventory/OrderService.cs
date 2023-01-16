using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Inventory;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;
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


        public Task<HttpResponseResult<PagedList<OrderList>>> ListAsync(PagingReqeustOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<PagedList<OrderList>>(BaseUrl, options.ConvertToDictionary(), cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<OrderList>>> ListByUserAsync(string userId,PagingReqeustOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<PagedList<OrderList>>(string.Format("{0}/user/{1}", BaseUrl, userId), options.ConvertToDictionary(), cancellationToken);
        }

        public Task<HttpResponseResult<Order>> RetrieveAsync(Guid orderId , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeGetRequest<Order>(string.Format("{0}/{1}", BaseUrl, orderId), cancellationToken: cancellationToken);
        }

        public Task<HttpResponseResult<Order>> RetrieveByExternalIdAsync(Guid externalOrderId, CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeGetRequest<Order>(string.Format("{0}/external_order_id/{1}", BaseUrl, externalOrderId), cancellationToken: cancellationToken);
        }


    }
}
