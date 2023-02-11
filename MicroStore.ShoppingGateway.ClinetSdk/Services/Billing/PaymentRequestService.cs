using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Billing
{
    public class PaymentRequestService
    {
        const string BaseUrl = "/payments";

        private readonly MicroStoreClinet _microStoreClinet;

        public PaymentRequestService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public Task<HttpResponseResult<PaymentRequest>> CreateAsync(PaymentCreateRequestOptions options , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeRequest<PaymentRequest>(BaseUrl, HttpMethod.Post, options, cancellationToken);
        }

        public async Task<HttpResponseResult<PaymentProcess>> ProcessAsync(Guid paymentId,PaymentProcessRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest< PaymentProcess>(string.Format("{0}/process/{1}", BaseUrl, paymentId), HttpMethod.Post, options, cancellationToken);
        }


        public async Task<HttpResponseResult<PaymentRequest>> CompleteAsync(Guid paymentId , PaymentCompleteRequestOptions options , CancellationToken cancellationToken = default)
        {

            return await _microStoreClinet.MakeRequest<PaymentRequest>(string.Format("{0}/complete/{1}", BaseUrl, paymentId), HttpMethod.Post, options, cancellationToken);
        }

        public async Task<HttpResponseResult<PagedList<PaymentRequestList>>> ListAsync(PagingReqeustOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PagedList<PaymentRequestList>>(BaseUrl, HttpMethod.Get, options,cancellationToken);
        }

        public async Task<HttpResponseResult<PagedList<PaymentRequestList>>> ListByUserAsync(string userId,PagingReqeustOptions options ,CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PagedList<PaymentRequestList>>(string.Format("{0}/user/{1}", BaseUrl, userId),HttpMethod.Get ,options, cancellationToken);
        }

        public async Task<HttpResponseResult<PaymentRequest>> Retrieve(Guid paymentId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentRequest>(string.Format("{0}/{1}",BaseUrl,paymentId), HttpMethod.Get,  cancellationToken);
        }

        public async Task<HttpResponseResult<PaymentRequest>> RetrieveByOrderAsync(string orderId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentRequest>(string.Format("{0}/order_id/{1}", BaseUrl, orderId), HttpMethod.Get ,cancellationToken: cancellationToken);
        }
    }
}
