using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class OrderService
    {
        const string BaseUrl = "/ordering/orders";

        private readonly MicroStoreClinet _microStoreClinet;

        public OrderService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Order>(BaseUrl, HttpMethod.Post, options, cancellationToken);
        }

        public async Task<PagedList<OrderList>> ListAsync(PagingReqeustOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PagedList<OrderList>>(BaseUrl,HttpMethod.Get, options, cancellationToken);  
        }

        public async Task<PagedList<OrderList>> ListByUserAsync(string userId,PagingReqeustOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest< PagedList<OrderList>>(string.Format("{0}/user/{1}", BaseUrl, userId), HttpMethod.Get, options, cancellationToken);
        }

        public async Task<Order> Retrieve(Guid orderId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Order>(string.Format("{0}/{1}",BaseUrl,orderId),HttpMethod.Get,cancellationToken);
        } 

        public async Task FullfillAsync(Guid orderId ,OrderFullfillRequestOptions options, CancellationToken cancellationToken = default)
        {
             await _microStoreClinet.MakeRequest(string.Format("{0}/fullfill/{1}", BaseUrl, orderId), HttpMethod.Post, options, cancellationToken);

        }

        public  async Task CompleteAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
             await _microStoreClinet.MakeRequest(string.Format("{0}/complete/{1}", BaseUrl, orderId), HttpMethod.Post, cancellationToken);
        }

        public async Task CancelAsync(Guid orderId , OrderCancelRequestOptions options , CancellationToken cancellationToken = default)
        {
             await _microStoreClinet.MakeRequest(string.Format("{0}/cancel/{1}", BaseUrl, orderId), HttpMethod.Post,options, cancellationToken);
        }

    }
}
