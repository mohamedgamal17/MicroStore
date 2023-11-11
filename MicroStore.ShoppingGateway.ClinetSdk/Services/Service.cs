using MicroStore.ShoppingGateway.ClinetSdk.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services
{
    public abstract class Service
    {
        protected MicroStoreClinet MicroStoreClinet { get;}

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }

        };
        protected Service(MicroStoreClinet microStoreClinet)
        {
            MicroStoreClinet = microStoreClinet;
        }

        public async Task<TResponse> MakeRequestAsync<TResponse>(string path, HttpMethod httpMethod, object request = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            var response = await MicroStoreClinet.ProcessRequestAsync(path, httpMethod, request, requestHeaderOptions, cancellationToken);

            return JsonConvert.DeserializeObject<TResponse>(response.Content, _jsonSerializerSettings);
        }

        public async Task MakeRequestAsync(string path, HttpMethod httpMethod, object request = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            await MicroStoreClinet.ProcessRequestAsync(path, httpMethod, request, requestHeaderOptions, cancellationToken);
        }

    }
}
