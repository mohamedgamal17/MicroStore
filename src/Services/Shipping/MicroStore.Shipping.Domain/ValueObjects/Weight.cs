using Ardalis.GuardClauses;
using MicroStore.Shipping.Domain.Common;
using MicroStore.Shipping.Domain.Extensions;

namespace MicroStore.Shipping.Domain.ValueObjects
{
    public class Weight : ValueObject<Weight>
    {
        public static readonly Weight Empty = new Weight(0, WeightUnit.Gram);

        private readonly double _value;

        private WeightUnit  _unit;
        public double Value => _value;
        public WeightUnit Unit => _unit;

        public bool IsEmpty => this == Empty;

        private Weight() { }
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

        public static Weight FromUnit(double value , string unit)
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


        public  static Weight ConvertToGram(Weight weight)
        {
            return weight.Unit switch
            {
                WeightUnit.KiloGram => FromGram(weight.Value *  WeightConversion.FromKiloGramToGram ),
                WeightUnit.Pound => FromGram(weight.Value * WeightConversion.FromPoundToGram ),
                WeightUnit.Gram => weight,
                _ => throw new InvalidOperationException("Invalid system weight unit")
            };
        }

        public static Weight ConvertToPound(Weight weight)
        {
            return weight.Unit switch
            {
                WeightUnit.Gram => FromPound(weight.Value / WeightConversion.FromPoundToGram),
                WeightUnit.KiloGram => FromPound(weight.Value * WeightConversion.FromKiloGramToPound),
                WeightUnit.Pound => weight,
                _ => throw new InvalidOperationException("Invalid system weight unit")
            };
        }

        public static Weight ConvertToKiloGram(Weight weight)
        {
            return weight.Unit switch
            {
                WeightUnit.Gram => FromKiloGram(weight.Value / WeightConversion.FromKiloGramToGram),
                WeightUnit.Pound => FromKiloGram(weight.Value / WeightConversion.FromKiloGramToPound),
                WeightUnit.KiloGram => weight,
                _ => throw new InvalidOperationException("Invalid system weight unit")         
            };
        }

  
        public static Weight operator +(Weight left, Weight right)
        {
            Guard.Against.DifferentWeightSystemUnit(left, right);

            return new Weight(left.Value + right.Value, left.Unit);
        }


        public static Weight operator -(Weight left , Weight right)
        {
            Guard.Against.DifferentWeightSystemUnit(left, right);

            return new Weight(left.Value - right.Value, left.Unit);
        }



        public static Weight operator *(Weight left, int right)
        {
            Guard.Against.NegativeOrZero(right);

            return new Weight(left.Value * right, left.Unit);
        }
    }

    public enum WeightUnit
    {
        Gram = 0,
        Pound = 5,
        KiloGram = 10,       
    }
}
