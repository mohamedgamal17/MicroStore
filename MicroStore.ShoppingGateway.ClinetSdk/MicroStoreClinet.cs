using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using System;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;
using Newtonsoft.Json.Converters;

namespace MicroStore.ShoppingGateway.ClinetSdk
{
    public class MicroStoreClinet 
    {

        const string DefaultMediaType = "application/json";

        const string AuthorizationSchema = "Bearer";

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {

         
        };

        private readonly HttpClient _httpClient;

        public MicroStoreClinet(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseResult<TResponse>> MakeRequest<TResponse>(string path, HttpMethod httpMethod, object request = null, CancellationToken cancellationToken = default)
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
                }
                else
                {
                    if (request != null)
                    {
                        httpRequest.Content = new StringContent(await SerializeObject(request), Encoding.UTF8, DefaultMediaType);
                    }
                }



                var httpResponseMessage = await _httpClient.SendAsync(httpRequest, cancellationToken);

                return await ConvertResponseResult<TResponse>(httpResponseMessage, cancellationToken);

            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<HttpResponseResult> MakeRequest(string path, HttpMethod httpMethod, object request = null, CancellationToken cancellationToken = default)
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


                var httpResponseMessage = await _httpClient.SendAsync(httpRequest, cancellationToken);

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
            return Task.FromResult(JsonConvert.DeserializeObject<T>(Json, _jsonSerializerSettings));
        }

        public async Task<HttpResponseResult<TResponse>> ConvertResponseResult<TResponse>(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
        {
            string content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return HttpResponseResult.Success(httpResponseMessage.StatusCode, await DeserializeObject<HttpEnvelopeResult<TResponse>>(content));
            }

            return HttpResponseResult.Failure<TResponse>(httpResponseMessage.StatusCode, await DeserializeObject<HttpEnvelopeResult>(content));
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

        private JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {

                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),

                },


            };

            settings.Converters.Add(new StringEnumConverter());


            return settings;
        }

    }

    
    public class HttpResponseResult
    {

        public bool IsSuccess { get; }
        public HttpStatusCode HttpStatusCode { get; }
        public HttpEnvelopeResult HttpEnvelopeResult { get; }
        public bool IsFailure => !IsSuccess;

        internal HttpResponseResult(bool isSuccess, HttpStatusCode httpStatusCode, HttpEnvelopeResult httpEnvelopeResult)
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

        public static HttpResponseResult<T> Failure<T>(HttpStatusCode httpStatusCode, HttpEnvelopeResult htttpEnvelopeResult)
        {
            return new HttpResponseResult<T>(false, httpStatusCode, htttpEnvelopeResult);
        }


    }


    public class HttpResponseResult<T> : HttpResponseResult
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
                    return ((HttpEnvelopeResult<T>)HttpEnvelopeResult).GetResult();
                }

                throw new InvalidOperationException("http result cannot retrieved");
            }
        }

    }

    [Serializable]
    public class HttpEnvelopeResult
    {
        public ErrorInfo Error { get; set; }    

    }

    [Serializable]
    public class HttpEnvelopeResult<T> : HttpEnvelopeResult
    {
        public T Result { get; set; }
        public T GetResult()
        {
            return (T)Result;
        }

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
