

using Ardalis.GuardClauses;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.Inventory.Domain.Extensions
{
    public static class GuardClausesResultExtensions
    {

        public static Result InvalidResult(this IGuardClause guardClause , Result result)
        {
            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error.ToString());
            }

            return result;
        }
    }
}
