using Ardalis.GuardClauses;
using MicroStore.Shipping.Domain.Common;
using MicroStore.Shipping.Domain.Extensions;
namespace MicroStore.Shipping.Domain.ValueObjects
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

        public static Dimension FromUnit(double width, double length, double height , string unit)
        {
            return new Dimension(width, length, height, unit.ConvertDimensionUnit());
        }


        public static Dimension ConvertToCentiMeter(Dimension dimension)
        {
            return dimension.Unit switch
            {
                DimensionUnit.Inch => FromCentiMeter(dimension.Width * DimensionConversion.FromInchToCentiMeter, dimension.Length *
                        DimensionConversion.FromInchToCentiMeter, dimension.Height * DimensionConversion.FromInchToCentiMeter),

                DimensionUnit.CentiMeter => dimension,

                _ => throw new InvalidOperationException("Invalid system dimension unit")
            };
        }

        public static Dimension ConvertToInch(Dimension dimension)
        {
            return dimension.Unit switch
            {
                DimensionUnit.CentiMeter => FromInch(dimension.Width / DimensionConversion.FromInchToCentiMeter, dimension.Length / DimensionConversion.FromInchToCentiMeter, dimension.Height / DimensionConversion.FromInchToCentiMeter),

                DimensionUnit.Inch => dimension,

                _ => throw new InvalidOperationException("Invalid system dimension unit")
            };
        }


        public static Dimension Estimate(Dimension left, Dimension right)
        {
            Guard.Against.DifferentDimensionSystemUnit(left, right);

            double lenght = left.Length > right.Length ? left.Length : right.Length;

            double width = left.Width > right.Width ? left.Width : right.Width;

            double height = left.Height + right.Height;

            return new Dimension(width, lenght, height, left.Unit);
        }


        public static Dimension operator +(Dimension left, Dimension right)
        {
            Guard.Against.DifferentDimensionSystemUnit(left, right);

            double lenght = left.Length + right.Length;

            double width = left.Width + right.Width;

            double height = left.Height + right.Height;

            return new Dimension(width, lenght, height, left.Unit);
        }

        public static Dimension operator -(Dimension left, Dimension right)
        {
            Guard.Against.DifferentDimensionSystemUnit(left, right);

            double lenght = left.Length - right.Length;

            double width = left.Width - right.Width;

            double height = left.Height - right.Height;

            return new Dimension(width, lenght, height, left.Unit);
        }
    }

    public enum DimensionUnit
    {
        CentiMeter = 0,
        Inch = 5
    }
}
