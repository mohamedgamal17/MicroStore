using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;
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


        public Task<HttpResponseResult<Shipment>> CreateAsync(ShipmentCreateRequestOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<ShipmentCreateRequestOptions, Shipment>(options, BaseUrl, HttpMethod.Post, cancellationToken);
        }

        public Task<HttpResponseResult<Shipment>> FullfillAsync(Guid shipmentId ,ShipmentFullfillRequestOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<ShipmentFullfillRequestOptions, Shipment>(options, string.Format("{0}/fullfill/{1}"), HttpMethod.Post, cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<ShipmentList>>> ListAsync(PagingReqeustOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<PagedList<ShipmentList>>(BaseUrl, options.ConvertToDictionary(), cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<ShipmentList>>> ListByUserAsync(string userId , PagingReqeustOptions options, CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeGetRequest<PagedList<ShipmentList>>(string.Format("{0}/user/{1}",BaseUrl,userId), options.ConvertToDictionary(), cancellationToken);
        }

        public Task<HttpResponseResult<Shipment>> Retrieve(Guid shipmentId , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<Shipment>(string.Format("{0}/{1}", BaseUrl, shipmentId), cancellationToken: cancellationToken);
        }
        public Task<HttpResponseResult<Shipment>> RetrieveByOrderId(string orderid, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<Shipment>(string.Format("{0}/order_id/{1}", BaseUrl, orderid), cancellationToken: cancellationToken);
        }

    }
}
