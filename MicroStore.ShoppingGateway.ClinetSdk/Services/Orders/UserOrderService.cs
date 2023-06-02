using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class UserOrderService
    {
        const string BaseUrl = "/ordering/user/orders";

        private readonly MicroStoreClinet _microStoreClinet;

        public UserOrderService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<Order> SubmitOrderAsync(OrderSubmitRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Order>(BaseUrl, HttpMethod.Post, options, cancellationToken);
        }

        public async Task<PagedList<Order>> ListAsync(PagingReqeustOptions options ,CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PagedList<Order>>(BaseUrl, HttpMethod.Get, options, cancellationToken);
        }

        public async Task<Order> Retrieve(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Order>(string.Format("{0}/{1}", BaseUrl, orderId), HttpMethod.Get, cancellationToken);
        }
    }
}
