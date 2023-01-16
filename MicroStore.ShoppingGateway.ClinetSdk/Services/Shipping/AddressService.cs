using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class AddressService
    {
        const string BaseUrl = "/addresses";

        private readonly MicroStoreClinet _microStoreClient;

        public AddressService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }

        public Task<HttpResponseResult<AddressValidation>> ValidateAsync(Address address , CancellationToken cancellationToken)
        {
            return _microStoreClient.MakeRequest<Address,AddressValidation>(address,BaseUrl + "/"+ "validate",HttpMethod.Post,cancellationToken);
        }
    }
}
