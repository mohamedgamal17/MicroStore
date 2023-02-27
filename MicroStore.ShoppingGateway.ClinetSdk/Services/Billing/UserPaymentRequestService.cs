using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Billing
{
    public class UserPaymentRequestService
    {
        const string BaseUrl = "/billing/user/payments";

        private readonly MicroStoreClinet _microStoreClinet;

        public UserPaymentRequestService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public Task<PaymentRequest> CreateAsync(PaymentRequestOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<PaymentRequest>(BaseUrl, HttpMethod.Post, options, cancellationToken);
        }
        public async Task<PaymentProcess> ProcessAsync(string paymentId, PaymentProcessRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentProcess>(string.Format("{0}/process/{1}", BaseUrl, paymentId), HttpMethod.Post, options, cancellationToken);
        }

        public async Task<PaymentRequest> CompleteAsync(string paymentId, PaymentCompleteRequestOptions options, CancellationToken cancellationToken = default)
        {

            return await _microStoreClinet.MakeRequest<PaymentRequest>(string.Format("{0}/complete/{1}", BaseUrl, paymentId), HttpMethod.Post, options, cancellationToken);
        }
        public async Task< PagedList<PaymentRequestList>> ListAsync(PagingReqeustOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PagedList<PaymentRequestList>>(BaseUrl, HttpMethod.Get, options, cancellationToken);
        }

        public async Task<PaymentRequest> Retrieve(string paymentId, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentRequest>(string.Format("{0}/{1}", BaseUrl, paymentId), HttpMethod.Get, cancellationToken);
        }

        public async Task<PaymentRequest> RetrieveByOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentRequest>(string.Format("{0}/order_id/{1}", BaseUrl, orderId), HttpMethod.Get, cancellationToken: cancellationToken);
        }

    }
}
