using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class OrderService : Service,
        ICreatable<Order, OrderCreateRequestOptions>,
        IListableWithPaging<Order, OrderListRequestOptions>
    {
        const string BaseUrl = "/ordering/orders";

        public OrderService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<Order> CreateAsync(OrderCreateRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Order>(BaseUrl, HttpMethod.Post, options,requestHeaderOptions ,cancellationToken);
        }

        public async Task FullfillAsync(Guid orderId, OrderFullfillRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            await MakeRequestAsync(string.Format("{0}/fullfill/{1}", BaseUrl, orderId), HttpMethod.Post, options, requestHeaderOptions,cancellationToken);
        }

        public async Task CompleteAsync(Guid orderId, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            await MakeRequestAsync(string.Format("{0}/complete/{1}", BaseUrl, orderId), HttpMethod.Post,requestHeaderOptions: requestHeaderOptions , cancellationToken: cancellationToken );
        }

        public async Task CancelAsync(Guid orderId, OrderCancelRequestOptions options = null,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            await MakeRequestAsync(string.Format("{0}/cancel/{1}", BaseUrl, orderId), HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<PagedList<Order>> ListAsync(OrderListRequestOptions options ,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PagedList<Order>>(BaseUrl,HttpMethod.Get, options,requestHeaderOptions: requestHeaderOptions , cancellationToken : cancellationToken);  
        }

        public async Task<PagedList<Order>> ListByUserAsync(string userId,PagingReqeustOptions options = null , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync< PagedList<Order>>(string.Format("{0}/user/{1}", BaseUrl, userId), HttpMethod.Get, options,requestHeaderOptions ,cancellationToken);
        }

        public async Task<Order> GetAsync(Guid orderId , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Order>(string.Format("{0}/{1}",BaseUrl,orderId),HttpMethod.Get, requestHeaderOptions: requestHeaderOptions,cancellationToken: cancellationToken);
        }

        public async Task<Order> GetByOrderNumberAsync(string orderNumber, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Order>(string.Format("{0}/order_number/{1}", orderNumber), HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }

    }
}
