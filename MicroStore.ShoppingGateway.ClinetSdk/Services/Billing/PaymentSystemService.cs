using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Billing
{
    public class PaymentSystemService 
    {
        const string BaseUrl = "/paymentsystems";

        private readonly MicroStoreClinet _microStoreClinet;

        public PaymentSystemService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public Task<HttpResponseResult> UpdateAsync(string sysName , PyamentSystemUpdateRequestOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest(options, BaseUrl, HttpMethod.Put, cancellationToken);
        } 

        public Task<HttpResponseResult<ListResult<PaymentSystem>>> ListAsync(CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<ListResult<PaymentSystem>>(BaseUrl, cancellationToken: cancellationToken);
        }

        public Task<HttpResponseResult<PaymentSystem>> RetrieveAsync(Guid sysId , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<PaymentSystem>(string.Format("{0}/{1}", BaseUrl, sysId), cancellationToken: cancellationToken); 
        }

        public Task<HttpResponseResult<PaymentSystem>> RetrieveByNameAsync(string sysName , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<PaymentSystem>(string.Format("{0}/system_name/{1}", BaseUrl, sysName), cancellationToken: cancellationToken);
        }

    }
}
