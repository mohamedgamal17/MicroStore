namespace MicroStore.Catalog.Domain.Const
{
    public static class StandardDimensionUnit
    {

        public static string CentiMeter => "cm";

        public static string Inch => "inch";


        public static List<string> GetStandardDimensionUnit()
        {
            return new List<string>
            {
                CentiMeter,
                Inch
            };
        }
    }
}
