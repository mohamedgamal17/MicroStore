using Ardalis.GuardClauses;
using MicroStore.Catalog.Domain.Common;
using MicroStore.Catalog.Domain.Extensions;
namespace MicroStore.Catalog.Domain.ValueObjects
{
    public class Dimension : ValueObject<Dimension>
    {
        public static readonly Dimension Empty = new Dimension(0, 0, 0, DimensionUnit.CentiMeter);

        private readonly double _width;

        private readonly double _length;

        private readonly double _height;

        private readonly DimensionUnit _unit;
        public double Width => _width;
        public double Length => _length;
        public double Height => _height;
        public DimensionUnit Unit => _unit;
        public bool IsEmpty => this == Empty;


        private Dimension() { }

        private Dimension(double width, double length, double height, DimensionUnit unit)
        {
            Guard.Against.Negative(width, nameof(width));
            Guard.Against.Negative(length, nameof(length));
            Guard.Against.Negative(height, nameof(height));
            _width = width;
            _length = length;
            _height = height;
            _unit = unit;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _width;
            yield return _length;
            yield return _height;
            yield return _unit;
        }

        public static Dimension FromCentiMeter(double width, double length, double height)
        {
            return new Dimension(width, length, height, DimensionUnit.CentiMeter);
        }

        public static Dimension FromInch(double width, double length, double height)
        {
            return new Dimension(width, length, height, DimensionUnit.Inch);
        }

        public static Dimension FromUnit(double width, double length, double height, string unit)
        {
            return new Dimension(width, length, height, unit.ConvertDimensionUnit());
        }
    }

    public enum DimensionUnit
    {        
        CentiMeter = 0,

        Inch = 5
    }
}
