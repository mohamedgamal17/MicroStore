using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Billing
{
    public class PaymentSystemService 
    {
        const string BaseUrl = "/billing/systems";

        private readonly MicroStoreClinet _microStoreClinet;

        public PaymentSystemService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<PaymentSystem> UpdateAsync(string sysName , PyamentSystemUpdateRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentSystem>(BaseUrl, HttpMethod.Put, options, cancellationToken);
        } 

        public async Task<List<PaymentSystem>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<List<PaymentSystem>>(BaseUrl,HttpMethod.Get ,cancellationToken: cancellationToken);
        }

        public async Task<PaymentSystem> RetrieveAsync(string sysId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentSystem>(string.Format("{0}/{1}", BaseUrl, sysId), HttpMethod.Get,cancellationToken: cancellationToken); 
        }

        public async Task<PaymentSystem> RetrieveByNameAsync(string sysName , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<PaymentSystem>(string.Format("{0}/system_name/{1}", BaseUrl, sysName), HttpMethod.Get, cancellationToken: cancellationToken);
        }

    }
}
