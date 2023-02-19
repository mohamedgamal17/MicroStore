using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;

namespace MicroStore.ShoppingGateway.ClinetSdk.Extensions
{
    public static class HttpResponseResultExtensions
    {
        public static void ThrowIfFailure(this HttpResponseResult result)
        {
            if (result.IsFailure)
            {
                throw new MicroStoreClientException(result.HttpStatusCode ,result?.HttpEnvelopeResult?.Error);
            }
        }
    }
}
