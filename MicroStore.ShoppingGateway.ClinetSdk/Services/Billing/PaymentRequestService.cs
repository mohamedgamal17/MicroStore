using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;

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
            return _microStoreClinet.MakeRequest<PaymentCreateRequestOptions, PaymentRequest>(options, BaseUrl, HttpMethod.Post, cancellationToken);
        }

        public Task<HttpResponseResult<PaymentProcess>> ProcessAsync(Guid paymentId,PaymentProcessRequestOptions options , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeRequest<PaymentProcessRequestOptions, PaymentProcess>(options, string.Format("{0}/process/{1}", BaseUrl, paymentId), HttpMethod.Post, cancellationToken);
        }


        public Task<HttpResponseResult<PaymentRequest>> CompleteAsync(Guid paymentId , PaymentCompleteRequestOptions options , CancellationToken cancellationToken)
        {

            return _microStoreClinet.MakeRequest<PaymentCompleteRequestOptions, PaymentRequest>(options, string.Format("{0}/complete/{1}", BaseUrl, paymentId), HttpMethod.Post, cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<PaymentRequestList>>> ListAsync(PagingReqeustOptions options , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeGetRequest<PagedList<PaymentRequestList>>(BaseUrl,options.ConvertToDictionary(), cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<PaymentRequestList>>> ListByUserAsync(string userId,PagingReqeustOptions options ,CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<PagedList<PaymentRequestList>>(string.Format("{0}/user/{1}", BaseUrl, userId), options.ConvertToDictionary(), cancellationToken);
        }

        public Task<HttpResponseResult<PaymentRequest>> Retrieve(Guid paymentId , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<PaymentRequest>(uri:string.Format("{0}/{1}",BaseUrl,paymentId),cancellationToken: cancellationToken);
        }

        public Task<HttpResponseResult<PaymentRequest>> RetrieveByOrderAsync(string orderId , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<PaymentRequest>(uri: string.Format("{0}/order_id/{1}", BaseUrl, orderId), cancellationToken: cancellationToken);
        }
    }
}
