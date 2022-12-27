using FluentAssertions;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Domain.Tests
{
    public class DimensionTests
    {
        [TestCase(.5, .8, .6, 1.27, 2.032, 1.524)]
        [TestCase(1.5, 1.8, 1.6, 3.81, 4.572, 4.064)]
        [TestCase(5, 8, 16, 12.7, 20.32, 40.64)]
        public void Should_convert_dimension_from_inch_to_centimeter_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
        {
            Dimension sut = Dimension.FromInch(width, lenght, height);

            Dimension result = Dimension.ConvertToCentiMeter(sut);

            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

            result.Unit.Should().Be(DimensionUnit.CentiMeter);
        }


        [TestCase(.5, .8, .6, 0.1968503937, 0.31496062992, 0.23622047244)]
        [TestCase(1.5, 1.8, 1.6, 0.5905511811, 0.70866141732, 0.62992125984)]
        [TestCase(5, 8, 16, 1.968503937, 3.1496062992, 6.2992125984)]
        public void Should_convert_dimension_from_centimeter_to_inch_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
        {
            Dimension sut = Dimension.FromCentiMeter(width, lenght, height);

            Dimension result = Dimension.ConvertToInch(sut);

            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

            result.Unit.Should().Be(DimensionUnit.Inch);
        }
    }
}
