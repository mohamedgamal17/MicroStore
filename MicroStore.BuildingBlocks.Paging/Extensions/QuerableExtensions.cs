using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
namespace MicroStore.BuildingBlocks.Paging.Extensions
{
    public static class QuerableExtensions
    {

        public static async Task<PagedResult<T>> PageResult<T>(this IQueryable<T> query,int pagenumber , int pagesize,CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(query, nameof(query));
            Guard.Against.NegativeOrZero(pagenumber, nameof(pagenumber));
            Guard.Against.NegativeOrZero(pagesize, nameof(pagesize));

            var count = await query.CountAsync();

            var items = await query
                .Skip( ((pagenumber -1) * pagesize))
                .Take(pagesize)
                .ToListAsync();

            return new PagedResult<T>(items, count,pagenumber,pagesize);

        }
    }
}
