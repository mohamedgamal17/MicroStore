using Elastic.Clients.Elasticsearch;
using MicroStore.BuildingBlocks.Utils.Paging;
namespace MicroStore.Catalog.Application.Extensions
{
    public static class ElasticSearchPagedResultExtensions
    {
        public static  PagedResult<T> ToPagedResult<T>(this SearchResponse<T> response,int skip , int size )
        {

            return new PagedResult<T>(response.Documents,response.Total, skip, size);
        }
    }
}
