using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Billing
{
    public class UserPaymentRequestService : Service,
        IListableWithPaging<PaymentRequest, PagingReqeustOptions>,
        ICreatable<PaymentRequest, PaymentRequestOptions>,
        IRetrievable<PaymentRequest>
    {
        const string BaseUrl = "/billing/user/payments";

        public UserPaymentRequestService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<PaymentRequest> CreateAsync(PaymentRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PaymentRequest>(BaseUrl, HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<PaymentProcess> ProcessAsync(string paymentId, PaymentProcessRequestOptions options, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PaymentProcess>(string.Format("{0}/process/{1}", BaseUrl, paymentId), HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<PagedList<PaymentRequest>> ListAsync(PagingReqeustOptions options, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PagedList<PaymentRequest>>(BaseUrl, HttpMethod.Get, options, requestHeaderOptions, cancellationToken);
        }


        public async Task<PaymentRequest> GetAsync(string id, RequestHeaderOptions requestHeaderOptions = null
        , CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PaymentRequest>(string.Format("{0}/{1}", BaseUrl, id), HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }

        public async Task<PaymentRequest> GetByOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PaymentRequest>(string.Format("{0}/order_id/{1}", BaseUrl, orderId), HttpMethod.Get, cancellationToken: cancellationToken);
        }

    }
}
