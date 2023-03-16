using Microsoft.Extensions.Options;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentService
    {
        const string BaseUrl = "/shipping/shipments";

        private readonly MicroStoreClinet _microStoreClinet;

        public ShipmentService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }


        public async Task<Shipment> CreateAsync(ShipmentCreateRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Shipment>(BaseUrl, HttpMethod.Post, options, cancellationToken);
        }

        public async Task<Shipment> FullfillAsync(string shipmentId ,ShipmentFullfillRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest< Shipment>( string.Format("{0}/fullfill/{1}", BaseUrl, shipmentId), HttpMethod.Post, options, cancellationToken);
        }

        public Task<PagedList<ShipmentList>> ListAsync(PagingReqeustOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<PagedList<ShipmentList>>(BaseUrl,HttpMethod.Get ,options, cancellationToken);
        }

        public async Task<PagedList<ShipmentList>> ListByUserAsync(string userId , PagingReqeustOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PagedList<ShipmentList>>(string.Format("{0}/user/{1}",BaseUrl,userId),HttpMethod.Get ,options, cancellationToken);
        }

        public async Task<Shipment> GetAsync(string shipmentId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Shipment>(string.Format("{0}/{1}", BaseUrl, shipmentId), HttpMethod.Get ,cancellationToken);
        }
        public async Task<Shipment> GetByOrderId(string orderid, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Shipment>(string.Format("{0}/order_id/{1}", BaseUrl, orderid), HttpMethod.Get , cancellationToken);
        }


        public async Task<List<ShipmentRate>> RetrieveRatesAsync(string shipmentId, CancellationToken
          cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<List<ShipmentRate>>(string.Format("{0}/{1}/rates",BaseUrl,shipmentId) , HttpMethod.Post, cancellationToken);
        }

        public async Task<Shipment> PurchaseLabelAsync(string shipmentId, PurchaseLabelRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Shipment>(string.Format("{0}/{1}/labels", BaseUrl, shipmentId), HttpMethod.Post, options,  cancellationToken);
        }
    }
}
