using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentService
    {
        const string BaseUrl = "/shipments";

        private readonly MicroStoreClinet _microStoreClinet;

        public ShipmentService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }


        public async Task<HttpResponseResult<Shipment>> CreateAsync(ShipmentCreateRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Shipment>(BaseUrl, HttpMethod.Post, options, cancellationToken);
        }

        public async Task<HttpResponseResult<Shipment>> FullfillAsync(Guid shipmentId ,ShipmentFullfillRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest< Shipment>( string.Format("{0}/fullfill/{1}"), HttpMethod.Post, options, cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<ShipmentList>>> ListAsync(PagingReqeustOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<PagedList<ShipmentList>>(BaseUrl,HttpMethod.Get ,options, cancellationToken);
        }

        public async Task<HttpResponseResult<PagedList<ShipmentList>>> ListByUserAsync(string userId , PagingReqeustOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PagedList<ShipmentList>>(string.Format("{0}/user/{1}",BaseUrl,userId),HttpMethod.Get ,options, cancellationToken);
        }

        public async Task<HttpResponseResult<Shipment>> Retrieve(Guid shipmentId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Shipment>(string.Format("{0}/{1}", BaseUrl, shipmentId), HttpMethod.Get ,cancellationToken);
        }
        public async Task<HttpResponseResult<Shipment>> RetrieveByOrderId(string orderid, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Shipment>(string.Format("{0}/order_id/{1}", BaseUrl, orderid), HttpMethod.Get , cancellationToken);
        }

    }
}
