namespace MicroStore.Shipping.Domain.Const
{
    public class StandardWeightUnit
    {
        public static string Gram => "g";
        public static string Pound => "lb";
        public static string KiloGram => "kg";

        public static List<string> FromValues()
        {
            return new List<string>
            {
                Gram,
                Pound,
                KiloGram
            };
        }
    }
}
