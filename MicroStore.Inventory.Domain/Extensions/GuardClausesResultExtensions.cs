using Ardalis.GuardClauses;
using MicroStore.BuildingBlocks.Utils.Results;
namespace MicroStore.Inventory.Domain.Extensions
{
    public static class GuardClausesResultExtensions
    {

        public static void InvalidResult<T>(this IGuardClause guardClause , Result<T> result , Type entityType)
        {
            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Exception.Message);
            }

        }
    }
}
