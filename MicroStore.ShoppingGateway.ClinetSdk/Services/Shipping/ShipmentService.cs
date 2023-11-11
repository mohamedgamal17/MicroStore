using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentService : Service,
        ICreatable<Shipment, ShipmentCreateRequestOptions>,
        IListableWithPaging<Shipment, PagingReqeustOptions>,
        IRetrievable<Shipment>
    {
        const string BaseUrl = "/shipping/shipments";

        public ShipmentService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<Shipment> CreateAsync(ShipmentCreateRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Shipment>(BaseUrl, HttpMethod.Post, options, requestHeaderOptions,cancellationToken);
        }

        public async Task<Shipment> FullfillAsync(string shipmentId ,ShipmentFullfillRequestOptions options , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Shipment>( string.Format("{0}/fullfill/{1}", BaseUrl, shipmentId), HttpMethod.Post, options,requestHeaderOptions ,cancellationToken);
        }

        public Task<PagedList<Shipment>> ListAsync(PagingReqeustOptions options = null, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return MakeRequestAsync<PagedList<Shipment>>(BaseUrl,HttpMethod.Get ,options, requestHeaderOptions, cancellationToken);
        }

        public async Task<PagedList<Shipment>> ListByUserAsync(string userId , PagingReqeustOptions options, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PagedList<Shipment>>(string.Format("{0}/user/{1}",BaseUrl,userId),HttpMethod.Get ,options, requestHeaderOptions, cancellationToken);
        }

        public async Task<Shipment> GetAsync(string shipmentId , RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Shipment>(string.Format("{0}/{1}", BaseUrl, shipmentId), HttpMethod.Get ,requestHeaderOptions: requestHeaderOptions,cancellationToken: cancellationToken);
        }
        public async Task<Shipment> GetByOrderId(string orderid, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Shipment>(string.Format("{0}/order_id/{1}", BaseUrl, orderid), HttpMethod.Get , requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }


        public async Task<List<ShipmentRate>> ListRatesAsync(string shipmentId, RequestHeaderOptions requestHeaderOptions = null,CancellationToken
          cancellationToken = default)
        {
            return await MakeRequestAsync<List<ShipmentRate>>(string.Format("{0}/{1}/rates",BaseUrl,shipmentId) , HttpMethod.Post, requestHeaderOptions: requestHeaderOptions , cancellationToken : cancellationToken);
        }

        public async Task<Shipment> PurchaseLabelAsync(string shipmentId, PurchaseLabelRequestOptions options , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Shipment>(string.Format("{0}/{1}/labels", BaseUrl, shipmentId), HttpMethod.Post, options, requestHeaderOptions , cancellationToken);
        }
    }
}
