using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class UserOrderService : Service ,
        IListableWithPaging<Order, PagingReqeustOptions>
    {
        const string BaseUrl = "/ordering/user/orders";

        private readonly MicroStoreClinet _microStoreClinet;

        public UserOrderService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<Order> SubmitOrderAsync(OrderSubmitRequestOptions options = null,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Order>(BaseUrl, HttpMethod.Post, options,requestHeaderOptions, cancellationToken);
        }

        public async Task<PagedList<Order>> ListAsync(PagingReqeustOptions options= null,RequestHeaderOptions  requestHeaderOptions = null , CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PagedList<Order>>(BaseUrl, HttpMethod.Get, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<Order> GetAsync(Guid orderId, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Order>(string.Format("{0}/{1}", BaseUrl, orderId), HttpMethod.Get, requestHeaderOptions: requestHeaderOptions ,cancellationToken:cancellationToken);
        }
    }
}
