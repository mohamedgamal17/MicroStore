namespace MicroStore.Catalog.Domain.ValueObjects
{
    public static class DimensionUnit
    {
        public static readonly string CentiMeter = "cm";

        public static readonly string Meter = "m";

        public static readonly string Inch = "inch";

        public static readonly string Feet = "ft";

        public static readonly string None = "none";


        public static List<string> AsList()
        {
            return new List<string>
            {
                CentiMeter,
                Meter,
                Inch,
                Feet
            };
        }

    }
}
