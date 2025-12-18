using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Billing
{
    public class PaymentSystemService  : Service, IListable<PaymentSystem>
    {
        const string BaseUrl = "/billing/systems";

        public PaymentSystemService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<List<PaymentSystem>> ListAsync(RequestHeaderOptions requestHeaderOptions = null,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<List<PaymentSystem>>(BaseUrl,HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }
    }
}
