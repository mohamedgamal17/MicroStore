using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static WeightUnit  ConvertWeightUnit(this string unit)
        {
            return Enum.Parse<WeightUnit>(unit, true);
        }

        public static DimensionUnit ConvertDimensionUnit (this string unit)
        {
            return Enum.Parse<DimensionUnit>(unit, true);
        }
    }
}
