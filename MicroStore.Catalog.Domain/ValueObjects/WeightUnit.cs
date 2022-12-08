namespace MicroStore.Catalog.Domain.ValueObjects
{
    public class WeightUnit
    {
        public static readonly string Gram = "g";

        public static readonly string KiloGram = "kg";

        public static readonly string Pound = "lb";

        public static readonly string None = "none";


        public static List<string> AsList()
        {
            return new List<string>
            {
                Gram,
                KiloGram,
                Pound
            };
        }
    }
}
