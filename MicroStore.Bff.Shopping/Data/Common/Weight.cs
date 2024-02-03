namespace MicroStore.Bff.Shopping.Data.Common
{
    public class Weight
    {
        public double Value { get; set; }
        public WeightUnit Unit { get; set; }
    }

    public enum WeightUnit
    {
        Gram = 0,
        Pound = 5,
        KilogGram = 10,
    }
}
