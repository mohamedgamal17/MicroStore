namespace MicroStore.Shipping.Domain.Const
{
    public class StandardWeightUnit
    {
        public static string Gram => "Gram";
        public static string Pound => "Pound";
        public static string KiloGram => "KiloGram";

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
