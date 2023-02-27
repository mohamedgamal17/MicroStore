using System.Net;
namespace MicroStore.ShoppingGateway.ClinetSdk.Exceptions
{
    [Serializable]
    public class MicroStoreClientException  : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public MicroStoreError Erorr { get; set; }

        public MicroStoreClientException(HttpStatusCode httpStatusCode, string message, MicroStoreError error = null)
            : base(message)
        {
            StatusCode = httpStatusCode;
            Erorr = error;

            
        }
    }
}
