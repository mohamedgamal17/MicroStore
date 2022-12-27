using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Domain.Extensions
{
    public static class EnumExtensions
    {

        public static WeightUnit  ConvertWeightUnit(this string unit)
        {
            if(unit == StandardWeightUnit.Gram)
            {
                return WeightUnit.Gram;
            }

            if(unit == StandardWeightUnit.Pound)
            {
                return WeightUnit.Pound;
            }

            if(unit == StandardWeightUnit.KiloGram)
            {
                return WeightUnit.KiloGram;
            }

            return WeightUnit.None;
        }


        public static DimensionUnit ConvertDimensionUnit (this string unit)
        {
            if(unit == StandardDimensionUnit.CentiMeter)
            {
                return DimensionUnit.CentiMeter;
            }

            if(unit == StandardDimensionUnit.Inch)
            {
                return DimensionUnit.Inch;
            }

            return DimensionUnit.None;
        }
    }
}
