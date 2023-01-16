using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
namespace MicroStore.ShoppingGateway.ClinetSdk
{
    public class MicroStoreClinet : HttpClient
    {

        const string DefaultMediaType = "application/json";

        const string AuthorizationSchema = "Bearer";

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(),

            },
        };

        private readonly MicroStoreClinetConfiguration _microStoreClinetConfiguration;

        private readonly IServiceProvider _serviceProvider;

        public MicroStoreClinet(MicroStoreClinetConfiguration microStoreClinetConfiguration, IServiceProvider serviceProvider)
        {

            _microStoreClinetConfiguration = microStoreClinetConfiguration;
            _serviceProvider = serviceProvider;

        }

        public async Task<HttpResponseResult<TResponse>> MakeGetRequest<TResponse>(string uri, Dictionary<string, string> queryString = null ,CancellationToken cancellationToken = default) 
        {
            try
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri(QueryHelpers.AddQueryString(uri, queryString)),
                    Method = HttpMethod.Get
                };

                if (_microStoreClinetConfiguration.TokenHandlerDeleagete != null)
                {
                    var token = await _microStoreClinetConfiguration.TokenHandlerDeleagete(_serviceProvider.CreateScope().ServiceProvider);

                    DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationSchema, token);
                }

                var httpResponseMessage = await SendAsync(httpRequest, cancellationToken);


                return await ConvertResponseResult<TResponse>(httpResponseMessage, cancellationToken);
            }
            catch(HttpRequestException ex)
            {
                throw ex;
            }


        }

        public async Task<HttpResponseResult<TResponse>> MakeRequest<TRequest,TResponse>(TRequest request, string uri , HttpMethod httpMethod , CancellationToken cancellationToken)
        {
            try
            {
                string json = await SerializeObject(request);

                HttpRequestMessage httpRequest = new HttpRequestMessage
                {
                    Content = new StringContent(json, Encoding.UTF8, DefaultMediaType),
                    Method = httpMethod,
                    RequestUri = new Uri(uri),
                };

                if(_microStoreClinetConfiguration.TokenHandlerDeleagete != null)
                {
                    var token = await _microStoreClinetConfiguration.TokenHandlerDeleagete(_serviceProvider.CreateScope().ServiceProvider);

                    DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationSchema, token);

                }

                var httpResponseMessage  = await SendAsync(httpRequest, cancellationToken);


                return await ConvertResponseResult<TResponse>(httpResponseMessage,cancellationToken);

                
            }catch(HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<HttpResponseResult> MakeRequest<TRequest>(TRequest request, string uri, HttpMethod httpMethod, CancellationToken cancellationToken)
        {
            try
            {
                string json = await SerializeObject(request);

                HttpRequestMessage httpRequest = new HttpRequestMessage
                {
                    Content = new StringContent(json, Encoding.UTF8, DefaultMediaType),
                    Method = httpMethod,
                    RequestUri = new Uri(uri),
                };

                if (_microStoreClinetConfiguration.TokenHandlerDeleagete != null)
                {
                    var token = await _microStoreClinetConfiguration.TokenHandlerDeleagete(_serviceProvider.CreateScope().ServiceProvider);

                    DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationSchema, token);

                }

                var httpResponseMessage = await SendAsync(httpRequest, cancellationToken);


                return await ConvertResponseResult(httpResponseMessage, cancellationToken);


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
            return Task.FromResult(JsonConvert.DeserializeObject<T>(Json,_jsonSerializerSettings));
        }

        public async Task<HttpResponseResult<TResponse>> ConvertResponseResult<TResponse>(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken )
        {
            string content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            var httpEnvelopeResult = await DeserializeObject<HttpEnvelopeResult<TResponse>>(content);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return HttpResponseResult.Success(httpResponseMessage.StatusCode, httpEnvelopeResult);
            }

            return HttpResponseResult.Failure(httpResponseMessage.StatusCode, httpEnvelopeResult);
        }


        public async Task<HttpResponseResult> ConvertResponseResult(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
        {
            string content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            var httpEnvelopeResult = await DeserializeObject<HttpEnvelopeResult>(content);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return HttpResponseResult.Success(httpResponseMessage.StatusCode, httpEnvelopeResult);
            }

            return HttpResponseResult.Failure(httpResponseMessage.StatusCode, httpEnvelopeResult);
        }

    }


    public class HttpResponseResult
    {
 
        public bool IsSuccess { get; }
        public HttpStatusCode HttpStatusCode { get; }
        public virtual HttpEnvelopeResult HttpEnvelopeResult { get; }
        public bool IsFailure => !IsSuccess;

        internal HttpResponseResult(bool isSuccess , HttpStatusCode httpStatusCode , HttpEnvelopeResult httpEnvelopeResult)
        {
            IsSuccess = isSuccess;
            HttpStatusCode = httpStatusCode;
            HttpEnvelopeResult = httpEnvelopeResult;
        }
        public static HttpResponseResult Success(HttpStatusCode httpStatusCode, HttpEnvelopeResult htttpEnvelopeResult)
        {
            return new HttpResponseResult(true, httpStatusCode, htttpEnvelopeResult); 
        }

        public static HttpResponseResult Failure(HttpStatusCode httpStatusCode, HttpEnvelopeResult htttpEnvelopeResult)
        {
            return new HttpResponseResult(false, httpStatusCode, htttpEnvelopeResult);
        }

        public static HttpResponseResult<T> Success<T>(HttpStatusCode httpStatusCode, HttpEnvelopeResult<T> htttpEnvelopeResult)
        {
            return new HttpResponseResult<T>(true, httpStatusCode, htttpEnvelopeResult);
        }        
        
        public static HttpResponseResult<T> Failure<T>(HttpStatusCode httpStatusCode, HttpEnvelopeResult<T> htttpEnvelopeResult)
        {
            return new HttpResponseResult<T>(false, httpStatusCode, htttpEnvelopeResult);
        }


    }

    public class HttpResponseResult <T> : HttpResponseResult
    {
        internal HttpResponseResult(bool isSuccess, HttpStatusCode httpStatusCode, HttpEnvelopeResult httpEnvelopeResult) : base(isSuccess, httpStatusCode, httpEnvelopeResult)
        {

        }

        public T Result
        {
            get
            {
                if (IsSuccess)
                {
                    return ((HttpEnvelopeResult<T>)HttpEnvelopeResult).Result;
                }

                throw new InvalidOperationException("http result cannot retrieved");
            }
        }

    }

    [Serializable]
    public class HttpEnvelopeResult
    {
        public ErrorInfo Error { get; set; }
        public DateTime TimeGenerated { get; set; }

    }

    [Serializable]
    public class HttpEnvelopeResult<T> : HttpEnvelopeResult
    { 
       public T Result { get; set; }

    }

    [Serializable]
    public class ErrorInfo
    {
        public string Type { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public ValidationErrorInfo[] ValidationErrors { get; set; }

    }

    [Serializable]
    public class ValidationErrorInfo
    {
        public string Message { get; set; }

        public string[] Members { get; set; }

        public ValidationErrorInfo()
        {

        }

        public ValidationErrorInfo(string message)
        {
            Message = message;
        }

        public ValidationErrorInfo(string message, string[] members)
            : this(message)
        {
            Members = members;
        }


        public ValidationErrorInfo(string message, string member)
            : this(message, new[] { member })
        {

        }
    }
}
