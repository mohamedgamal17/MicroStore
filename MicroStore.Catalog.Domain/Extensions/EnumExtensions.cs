using MicroStore.Catalog.Domain.Const;
using MicroStore.Catalog.Domain.ValueObjects;

namespace MicroStore.Catalog.Domain.Extensions
{
    public static class EnumExtensions
    {

        public static WeightUnit ConvertWeightUnit(this string unit)
        {
            return Enum.Parse<WeightUnit>(unit, true);               
        }

        public static DimensionUnit ConvertDimensionUnit(this string unit)
        {
            return Enum.Parse<DimensionUnit>(unit, true);
        }
    }
}
