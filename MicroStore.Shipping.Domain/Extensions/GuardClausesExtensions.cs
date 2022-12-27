using Ardalis.GuardClauses;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Domain.Extensions
{
    public static class GuardClausesExtensions
    {

        public static void DifferentWeightSystemUnit(this IGuardClause guardClause , Weight arg0 , Weight arg1)
        {
            if(arg0.Unit != arg1.Unit)
            {
                throw new InvalidOperationException("Invalid system weight unit");
            }
        }

        public static void DifferentDimensionSystemUnit(this IGuardClause guardClause, Dimension arg0 , Dimension arg1)
        {
            if(arg0.Unit != arg1.Unit)
            {
                throw new InvalidOperationException("Invalid system dimension unit");
            }
        }
    }
}
