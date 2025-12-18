using Elastic.Clients.Elasticsearch;
using Elastic.Transport.Products.Elasticsearch;

namespace MicroStore.Catalog.Application.Operations.Extensions
{
    public static class ElasticResponseExtensions
    {
        public static void ThrowIfFailure(this ElasticsearchResponse response)
        {
            if (!response.IsValidResponse)
            {
                throw new InvalidOperationException(response.ApiCallDetails.ToString());
            }
        }
    }
}
