using Ardalis.GuardClauses;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.Payment.Application.Extensions
{
    public static class GuardClausesResultExtensions
    {

        public static Result InvalidResult(this IGuardClause guardClause, Result result)
        {
            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result;
        }
    }
}
