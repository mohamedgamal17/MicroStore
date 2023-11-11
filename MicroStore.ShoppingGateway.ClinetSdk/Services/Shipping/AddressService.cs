using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class AddressService : Service
    {
        const string BaseUrl = "/shipping/addresses";

        public AddressService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<AddressValidation> ValidateAsync(Address address ,RequestHeaderOptions requestHeaderOptions = null  ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<AddressValidation>(BaseUrl + "/"+ "validate",HttpMethod.Post, address, requestHeaderOptions,cancellationToken);
        }
    }
}
