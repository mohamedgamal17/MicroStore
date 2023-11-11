using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling
{
    public class UserAddressService : Service,
        ICreatable<Address, AddressRequestOptions>,
        IUpdateable<Address, AddressRequestOptions>,
        IDeletable,
        IListable<Address>,
        IRetrievable<Address>
    {
        const string BASE_URL = "/profiling/user/profile/addresses";
        public UserAddressService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<Address> CreateAsync(AddressRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Address>(BASE_URL, HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<Address> UpdateAsync(string id, AddressRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BASE_URL, id);

            return await MakeRequestAsync<Address>(path, HttpMethod.Put, options, requestHeaderOptions, cancellationToken);

        }

        public async Task DeleteAsync(string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BASE_URL, id);

            await MakeRequestAsync(path, HttpMethod.Delete, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }

        public async Task<Address> GetAsync(string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BASE_URL, id);

            return await MakeRequestAsync<Address>(path, HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }

        public async Task<List<Address>> ListAsync(RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<List<Address>>(BASE_URL, HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }


    }
}
