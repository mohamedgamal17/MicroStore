using MicroStore.Catalog.Domain.Common;
using System.Runtime.CompilerServices;

namespace MicroStore.Catalog.Domain.ValueObjects
{
    public class Weight : ValueObject<Weight>
    {
        public static readonly Weight Empty = new Weight(0, WeightUnit.None);

        private readonly double _value;

        private readonly string _unit;
        public double Value => _value;
        public string Unit => _unit;

        private Weight(double value, string unit)
        {
            _value = value;
            _unit = unit;
        }

        public static Weight FromGram(double value)
        {
            return new Weight(value, WeightUnit.Gram);
        }

        public static Weight FromKiloGram(double value)
        {
            return new Weight(value, WeightUnit.KiloGram);
        }

        public static Weight FromPound(double value)
        {
            return new Weight(value, WeightUnit.Pound);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
            yield return _unit;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode() + _unit.GetHashCode();
        }

        
    }
}
