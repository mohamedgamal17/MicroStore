using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class OrderService
    {
        const string BaseUrl = "/orders";

        private readonly MicroStoreClinet _microStoreClinet;

        public OrderService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public Task<HttpResponseResult<Order>> SubmitOrderAsync(OrderSubmitRequestOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<OrderSubmitRequestOptions,Order>(options,string.Format("{0}/{1}",BaseUrl,"submit"),HttpMethod.Post,cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<OrderList>>> ListAsync(PagingReqeustOptions options , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeRequest<PagingReqeustOptions,PagedList<OrderList>>(options,BaseUrl,HttpMethod.Get,cancellationToken);  
        }

        public Task<HttpResponseResult<PagedList<OrderList>>> ListByUserAsync(string userId,PagingReqeustOptions options , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeRequest<PagingReqeustOptions, PagedList<OrderList>>(options, string.Format("{0}/user/{1}", BaseUrl, userId), HttpMethod.Get, cancellationToken);
        }

        public Task<HttpResponseResult<Order>> Retrieve(Guid orderId , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<EmptyRequst,Order>(EmptyRequst.Empty,string.Format("{0}/{1}",BaseUrl,orderId),HttpMethod.Get,cancellationToken);
        } 

        public Task<HttpResponseResult> FullfillAsync(Guid orderId ,OrderFullfillRequestOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest(options, string.Format("{0}/fullfill/{1}", BaseUrl, orderId), HttpMethod.Post, cancellationToken);

        }

        public Task<HttpResponseResult> CompleteAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest(EmptyRequst.Empty, string.Format("{0}/complete/{1}", BaseUrl, orderId), HttpMethod.Post, cancellationToken);
        }

        public Task<HttpResponseResult> CancelAsync(Guid orderId , OrderCancelRequestOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest(options, string.Format("{0}/cancel/{1}", BaseUrl, orderId), HttpMethod.Post, cancellationToken);
        }

    }
}
