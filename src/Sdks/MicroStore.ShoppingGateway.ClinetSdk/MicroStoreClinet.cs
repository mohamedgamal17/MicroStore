using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using System.Net.Http.Headers;
namespace MicroStore.ShoppingGateway.ClinetSdk
{
    public class MicroStoreClinet
    {

        const string DefaultMediaType = "application/json";

        const string UnauthorizedMessage = "Unauthorized";

        const string ForbiddenMessage = "Forbidden";

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },

            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly HttpClient _httpClient;

        private readonly ILogger<MicroStoreClinet> _logger;

        public MicroStoreClinet(HttpClient httpClient, ILogger<MicroStoreClinet> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<MicroStoreResponse> ProcessRequestAsync(string path, HttpMethod httpMethod,object request = null, RequestHeaderOptions requestHeaderOptions = null,CancellationToken cancellationToken = default)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(string.Format("{0}{1}", _httpClient.BaseAddress?.AbsoluteUri, path)),
                Method = httpMethod
            };

            if (httpMethod == HttpMethod.Get)
            {
                if (request != null)
                {
                    httpRequest.RequestUri = new Uri(QueryHelpers.AddQueryString(httpRequest.RequestUri.AbsoluteUri, request.ConvertToDictionary()));
                }
            }
            else
            {
                if (request != null)
                {
                    httpRequest.Content = new StringContent(SerializeObject(request), Encoding.UTF8, DefaultMediaType);
                }             
            }

            if(requestHeaderOptions != null)
            {

                if(requestHeaderOptions.Authorization != null)
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", requestHeaderOptions.Authorization);
                }

            }


            _logger.LogInformation("Sending request with http method : {@httpMethod}  & request options : {@request}", httpMethod, SerializeObject(request));

            var httpResponseMessage = await _httpClient.SendAsync(httpRequest,HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            await ThrowIfFailureResponse(httpResponseMessage);

            return await httpResponseMessage.ToMicroStoreResponse();
        }


        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, _jsonSerializerSettings);
        }

        public T DeserializeObject<T>(string Json)
        {
            return JsonConvert.DeserializeObject<T>(Json, _jsonSerializerSettings);
        }

        public async Task<TResponse> ConvertResponseResult<TResponse>(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
        {

            string content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation("respoonse {@response}", content);

            return  DeserializeObject<TResponse>(content);
        }

        private async Task ThrowIfFailureResponse(HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var json = await httpResponseMessage.Content.ReadAsStringAsync();

                _logger.LogWarning("Error respoonse {@response}", json);

                var error =  DeserializeObject<MicroStoreError>(json);

                throw new MicroStoreClientException(httpResponseMessage.StatusCode, error?.Title ?? httpResponseMessage.StatusCode.ToString(), error);
            }
        }
    }



    [Serializable]
    public class MicroStoreError
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }


}
