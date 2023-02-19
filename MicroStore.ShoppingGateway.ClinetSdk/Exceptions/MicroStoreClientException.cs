using System.Net;
namespace MicroStore.ShoppingGateway.ClinetSdk.Exceptions
{
    public class MicroStoreClientException  : Exception
    {
        public ErrorInfo Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public MicroStoreClientException(HttpStatusCode statusCode, ErrorInfo error = null)
            :base(error?.Message)
        {
            Error = error;
            StatusCode = statusCode;
        }
    }
}
