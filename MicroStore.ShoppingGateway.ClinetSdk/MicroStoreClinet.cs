using Newtonsoft.Json;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using Microsoft.Extensions.Logging;

namespace MicroStore.ShoppingGateway.ClinetSdk
{
    public class MicroStoreClinet 
    {

        const string DefaultMediaType = "application/json";

        const string UnauthorizedMessage = "Unauthorized";

        const string ForbiddenMessage = "Forbidden";

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {

         
        };

        private readonly HttpClient _httpClient;

        private readonly ILogger<MicroStoreClinet> _logger;

        public MicroStoreClinet(HttpClient httpClient, ILogger<MicroStoreClinet> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TResponse> MakeRequest<TResponse>(string path, HttpMethod httpMethod, object request = null, CancellationToken cancellationToken = default)
        {

            try
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri(string.Format("{0}{1}", _httpClient.BaseAddress?.AbsoluteUri, path)),
                    Method = httpMethod
                };

                if (httpMethod == HttpMethod.Get)
                {
                    httpRequest.RequestUri = new Uri(QueryHelpers.AddQueryString(httpRequest.RequestUri.AbsoluteUri, request.ConvertToDictionary()));

                    _logger.LogInformation("Sending request with http method : {@httpMethod}  & request query : {request}", httpMethod, request);
                }
                else
                {
                    if (request != null)
                    {
                        httpRequest.Content = new StringContent(await SerializeObject(request), Encoding.UTF8, DefaultMediaType);
                    }

                    _logger.LogInformation("Sending request with http method : {@httpMethod}  & request query : {@request}", httpMethod, request);
                }

   

                var httpResponseMessage = await _httpClient.SendAsync(httpRequest, cancellationToken);

                ThrowIfFailureResponse(httpResponseMessage);


                return await ConvertResponseResult<TResponse>(httpResponseMessage, cancellationToken);

            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task MakeRequest(string path, HttpMethod httpMethod, object request = null, CancellationToken cancellationToken = default)
        {

            try
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri(string.Format("{0}{1}", _httpClient.BaseAddress?.AbsoluteUri , path)),
                    Method = httpMethod
                };

                if (request != null)
                {
                    httpRequest.Content = new StringContent(await SerializeObject(request), Encoding.UTF8, DefaultMediaType);
                }

                _logger.LogInformation("Sending request with http method : {@HttpMethod} & request body : {@Request}", httpMethod, request);


                var httpResponseMessage = await _httpClient.SendAsync(httpRequest, cancellationToken);

                ThrowIfFailureResponse(httpResponseMessage);

            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

     

       
        public Task<string> SerializeObject(object obj)
        {
            return Task.FromResult(JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
        }

        public Task<T> DeserializeObject<T>(string Json)
        {
            return Task.FromResult(JsonConvert.DeserializeObject<T>(Json, _jsonSerializerSettings));
        }

        public async Task<TResponse> ConvertResponseResult<TResponse>(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
        {

            string content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation("respoonse {@response}", content);

            return await DeserializeObject<TResponse>(content);
        }

        private async void ThrowIfFailureResponse(HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var json = await httpResponseMessage.Content.ReadAsStringAsync();

                _logger.LogInformation("Error respoonse {@response}", json);     

                var error = await DeserializeObject<MicroStoreError>(json);

                throw new MicroStoreClientException(httpResponseMessage.StatusCode, error?.Title ?? httpResponseMessage.StatusCode.ToString(), error);
            }
        }



    }



    [Serializable]
    public class MicroStoreError
    {
        public string Type { get; set; }
        public string Title {get ; set;}
        public string Detail { get; set; }
        public string Instance { get; set; }
        public Dictionary<string,string[]> Errors { get; set; }
    }


}
