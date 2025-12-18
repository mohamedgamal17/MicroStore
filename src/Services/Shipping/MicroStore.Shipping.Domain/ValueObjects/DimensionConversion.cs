namespace MicroStore.Shipping.Domain.ValueObjects
{
    public static class DimensionConversion
    {
        public static double FromMeterToFeet => 3.28084;
        public static double FromMeterToInch => 39.37;
        public static double FromMeterToCentiMeter => 100;
        public static double FromFeetToInch => 12;
        public static double FromFeetToCentiMeter => 30.48;
        public static double FromInchToCentiMeter => 2.54;
    }
}
