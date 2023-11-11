using System.Net.Http.Headers;
using System.Net;

namespace MicroStore.ShoppingGateway.ClinetSdk.Common
{
    public class MicroStoreResponse
    {
        public MicroStoreResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, string content)
        {
            StatusCode = statusCode;
            Headers = headers;
            Content = content;
        }

        public HttpStatusCode StatusCode { get; }

        public HttpResponseHeaders Headers { get; }

        public string Content { get; set; }
        public DateTimeOffset? Date => Headers?.Date;

        
    }
}
