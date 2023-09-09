using Elastic.Clients.Elasticsearch;
using MicroStore.BuildingBlocks.Paging;

namespace MicroStore.Catalog.Application.Extensions
{
    public static class ElasticSearchPagedResultExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this SearchResponse<T> response,int skip , int size ,ElasticsearchClient elasticsearchClient)
        {
            var countResponse = await elasticsearchClient.CountAsync<T>(desc=> desc.Query(qr=> qr.MatchAll()));

            return new PagedResult<T>(response.Documents, countResponse.Count, skip, size);
        }
    }
}
