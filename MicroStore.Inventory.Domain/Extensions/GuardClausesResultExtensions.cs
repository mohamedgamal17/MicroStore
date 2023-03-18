using Ardalis.GuardClauses;
using MicroStore.BuildingBlocks.Results;
namespace MicroStore.Inventory.Domain.Extensions
{
    public static class GuardClausesResultExtensions
    {

        public static void InvalidResult<T>(this IGuardClause guardClause , ResultV2<T> result , Type entityType)
        {
            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Exception.Message);
            }

        }
    }
}
