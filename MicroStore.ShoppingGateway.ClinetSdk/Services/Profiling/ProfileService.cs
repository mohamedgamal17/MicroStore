using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling
{
    public class ProfileService
    {
        const string BaseUrl = "/profiling/profile";

        private readonly MicroStoreClinet _microStoreClient;

        public ProfileService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }

        public async Task<PagedList<User>> ListAsync(PagingReqeustOptions options,CancellationToken cancellationToken = default)
        {
       
            return await _microStoreClient.MakeRequest<PagedList<User>>(BaseUrl, HttpMethod.Get,options,cancellationToken);
        }
        public async Task<User> GetUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BaseUrl, userId);

            return await _microStoreClient.MakeRequest<User>(path, HttpMethod.Get, cancellationToken: cancellationToken);
        }



    }
}
