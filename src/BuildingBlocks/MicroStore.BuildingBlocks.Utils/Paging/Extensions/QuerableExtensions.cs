using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Paging;
namespace MicroStore.BuildingBlocks.Utils.Paging.Extensions
{
    public static class QuerableExtensions
    {

        public static async Task<PagedResult<T>> PageResult<T>(this IQueryable<T> query, int skip, int lenght, CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(query, nameof(query));
            Guard.Against.Negative(skip, nameof(skip));
            Guard.Against.NegativeOrZero(lenght, nameof(lenght));

            var count = await query.CountAsync();

            var items = await query
                .Skip(skip)
                .Take(lenght)
                .ToListAsync();

            return new PagedResult<T>(items, count, skip, lenght);

        }
    }
}
