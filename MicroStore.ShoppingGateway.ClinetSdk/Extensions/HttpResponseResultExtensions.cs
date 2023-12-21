using MicroStore.ShoppingGateway.ClinetSdk.Common;
using System.Runtime.CompilerServices;

namespace MicroStore.ShoppingGateway.ClinetSdk.Extensions
{
    public static class HttpResponseResultExtensions
    {
        public static async Task<MicroStoreResponse> ToMicroStoreResponse(this HttpResponseMessage httpResponseMessage)
        {
            try
            {
                var httpStatusCode = httpResponseMessage.StatusCode;
                var headers = httpResponseMessage.Headers;
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                return new MicroStoreResponse(httpStatusCode, headers, content);
            }
            finally
            {
                httpResponseMessage.Dispose();
            }       
        }
    }
}
