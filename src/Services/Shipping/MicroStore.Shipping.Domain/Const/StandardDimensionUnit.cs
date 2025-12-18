namespace MicroStore.Shipping.Domain.Const
{
    public static class StandardDimensionUnit
    {

        public static string CentiMeter => "cm";

        public static string Inch => "inch";

        public static List<string> FromValues()
        {
            return new List<string>
            {
                CentiMeter,
                Inch,
            };
        }
    }
}
