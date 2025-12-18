namespace MicroStore.Bff.Shopping.Data.Common
{
    public class Dimension
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public DimensionUnit Unit { get; set; }
    }

    public enum DimensionUnit
    {
        CentiMeter = 0,

        Inch = 5
    }
}
