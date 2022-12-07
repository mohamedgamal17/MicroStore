using MicroStore.Catalog.Domain.Common;

namespace MicroStore.Catalog.Domain.ValueObjects
{
    public class Dimension : ValueObject<Dimension>
    {
        private readonly double _value;

        private readonly string _unit;
        public double Value => _value;
        public string Unit => _unit;

        private Dimension(double value, string unit)
        {
            _value = value;
            _unit = unit;
        }

        private Dimension() { }
      
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
            yield return _unit;
        }

        public static Dimension FromCentiMeter(double value)
        {
            return new Dimension(value, DimensionUnit.CentiMeter);
        }

        public static Dimension FromMeter(double value)
        {
            return new Dimension(value, DimensionUnit.Meter);
        }

        public static Dimension FromInc(double value)
        {
            return new Dimension(value, DimensionUnit.Inch);
        }

        public static Dimension FromFeet(double value)
        {
            return new Dimension(value, DimensionUnit.Feet);
        }
    }
}
