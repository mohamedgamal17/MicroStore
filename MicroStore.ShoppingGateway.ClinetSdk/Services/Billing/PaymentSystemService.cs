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

        public async Task<HttpResponseResult<PaymentSystem>> UpdateAsync(string sysName , PyamentSystemUpdateRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentSystem>(BaseUrl, HttpMethod.Put, options, cancellationToken);
        } 

        public async Task<HttpResponseResult<ListResult<PaymentSystem>>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<ListResult<PaymentSystem>>(BaseUrl,HttpMethod.Get ,cancellationToken: cancellationToken);
        }

        public async Task<HttpResponseResult<PaymentSystem>> RetrieveAsync(Guid sysId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentSystem>(string.Format("{0}/{1}", BaseUrl, sysId), HttpMethod.Get,cancellationToken: cancellationToken); 
        }

        public async Task<HttpResponseResult<PaymentSystem>> RetrieveByNameAsync(string sysName , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentSystem>(string.Format("{0}/system_name/{1}", BaseUrl, sysName), HttpMethod.Get, cancellationToken: cancellationToken);
        }

    }
}
