using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling
{
    public class UserProfileService : Service
    {
        const string BaseUrl = "/profiling/user/profile";


        public UserProfileService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<User> CreateAsync(ProfileRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
             return await MakeRequestAsync<User>(BaseUrl,HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<User> UpdateAsync(ProfileRequestOptions options,RequestHeaderOptions requestHeaderOptions =  null ,CancellationToken cancellationToken =  default)
        {
            return await MakeRequestAsync<User>(BaseUrl,HttpMethod.Put,options,requestHeaderOptions ,cancellationToken); 
        }

        public async Task<User> GetAsync(RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<User>(BaseUrl,HttpMethod.Get,requestHeaderOptions: requestHeaderOptions,cancellationToken: cancellationToken);
        }
    }
}
