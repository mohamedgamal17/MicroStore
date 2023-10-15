using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling
{
    public class UserProfileService
    {
        const string BaseUrl = "/profiling/user/profile";

        private readonly MicroStoreClinet _microStoreClient;

        public UserProfileService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }

        public async Task<User> CreateProfileAsync(ProfileRequestOptions options, CancellationToken cancellationToken = default)
        {
             return await _microStoreClient.MakeRequest<User>(BaseUrl,HttpMethod.Post, options, cancellationToken);
        }


        public async Task<User> UpdateProfileAsync(ProfileRequestOptions options, CancellationToken cancellationToken =  default)
        {
            return await _microStoreClient.MakeRequest<User>(BaseUrl,HttpMethod.Put,options, cancellationToken); 
        }


        public async Task<User> GetProfileAsync(CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<User>(BaseUrl,HttpMethod.Get,cancellationToken);
        }

        public async Task<List<Address>> ListAddressesAsync(CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BaseUrl, "addresses");

            return await _microStoreClient.MakeRequest<List<Address>>(path,HttpMethod.Get,cancellationToken);
        } 

        public async Task<Address> GetAddressAsync(string id , CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BaseUrl, "addresses", id);

            return await _microStoreClient.MakeRequest<Address>(path,HttpMethod.Get,cancellationToken);
        }

        public async Task<Address> CreateAddressAsync(AddressRequestOptions options,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BaseUrl, "addresses");

            return await _microStoreClient.MakeRequest<Address>(path,HttpMethod.Post,options, cancellationToken);
        }

        public async Task<Address> UpdateAddressAsync(string id , AddressRequestOptions options,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BaseUrl, "addresses", id);

            return await _microStoreClient.MakeRequest<Address>(path,HttpMethod.Put,options,cancellationToken);
        }

        public async Task<Address> RemoveAddressAsync(string id , CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BaseUrl, "addresses", id);

            return await _microStoreClient.MakeRequest<Address>(path, HttpMethod.Delete, cancellationToken: cancellationToken);
        }

    }
}
