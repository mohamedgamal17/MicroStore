using Ardalis.GuardClauses;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Domain.Exceptions;

namespace MicroStore.Inventory.Domain.Extensions
{
    public static class GuardClausesResultExtensions
    {

        public static Result InvalidResult(this IGuardClause guardClause , UnitResult result , Type entityType)
        {
            if (result.IsFailure)
            {
                throw new InvalidDomainException(entityType,result.ErrorType,result.Error.ToString()!);
            }

            return result;
        }
    }
}
