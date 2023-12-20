using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling
{
    public class ProfileService : Service,
        IListableWithPaging<User, PagingReqeustOptions>,
        IRetrievable<User>
    {
        const string BaseUrl = "/profiling/profiles";

        public ProfileService(MicroStoreClinet microStoreClinet) 
            : base(microStoreClinet)
        {
        }

        public async Task<PagedList<User>> ListAsync(PagingReqeustOptions options,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {     
            return await MakeRequestAsync<PagedList<User>>(BaseUrl, HttpMethod.Get,options, requestHeaderOptions, cancellationToken);
        }
        public async Task<User> GetAsync(string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BaseUrl, id);

            return await MakeRequestAsync<User>(path, HttpMethod.Get,requestHeaderOptions: requestHeaderOptions ,cancellationToken: cancellationToken);
        }
    }
}
