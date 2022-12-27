using Ardalis.GuardClauses;
using MicroStore.Catalog.Domain.Common;
using MicroStore.Catalog.Domain.Extensions;
namespace MicroStore.Catalog.Domain.ValueObjects
{
    public class Weight : ValueObject<Weight>
    {
        public static readonly Weight Empty = new Weight(0, WeightUnit.None);

        private readonly double _value;

        private WeightUnit _unit;
        public double Value => _value;
        public WeightUnit Unit => _unit;

        public bool IsEmpty => this == Empty;
        private Weight(double value, WeightUnit unit)
        {
            Guard.Against.Negative(value, nameof(value));
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

        public static Weight FromUnit(double value, string unit)
        {
            return new Weight(value, unit.ConvertWeightUnit());

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

    public enum WeightUnit
    {
        None = 0,
        Gram = 5,
        KiloGram = 10,
        Pound = 15,
    }
}
