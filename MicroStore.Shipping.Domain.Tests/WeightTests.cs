using FluentAssertions;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Domain.Tests
{
    [TestFixture]
    public class WeightTests
    {
        [TestCase(4.5,4500)]
        [TestCase(5,5000)]
        [TestCase(10,10000)]
        [TestCase(.1,100)]
        public void Should_convert_weight_from_kilogram_to_gram_unit(double input, double expectedValue)
        {
            Weight sut = Weight.FromKiloGram(input);

            Weight result = Weight.ConvertToGram(sut);

            result.Value.Should().BeApproximately(expectedValue, UnitConvertionConst.ApproxaimationValue);

            result.Unit.Should().Be(WeightUnit.Gram);
        }

        [TestCase(1.5, 680.388555)]
        [TestCase(.36, 163.2932532)]
        [TestCase(25.3641, 11504.962232)]
        [TestCase(3.6, 1632.932532)]
        public void Should_convert_weight_from_pound_to_gram_unit(double input , double expectedValue)
        {
            Weight sut = Weight.FromPound(input);

            Weight result = Weight.ConvertToGram(sut);

            result.Value.Should().BeApproximately(expectedValue, UnitConvertionConst.ApproxaimationValue);

            result.Unit.Should().Be(WeightUnit.Gram);
        }

        [TestCase(1, 2.205)]
        [TestCase(.6, 1.323)]
        [TestCase(3.145, 6.934)]
        [TestCase(20.5663, 45.34)]
        public void Should_convert_weight_from_kilogram_to_pound_unit(double input , double expectedValue)
        {
            Weight sut = Weight.FromKiloGram(input);

            Weight result = Weight.ConvertToPound(sut);

            result.Value.Should().BeApproximately(expectedValue, UnitConvertionConst.ApproxaimationValue);

            result.Unit.Should().Be(WeightUnit.Pound);        
        }

        [TestCase(1, 0.0022046226218)]
        [TestCase(3.56, 0.0078484565338)]
        [TestCase(10023, 22.096932539)]
        [TestCase(15003.32, 33.076658675)]
        public void Should_convert_weight_from_gram_to_pound_unit(double input, double expectedValue)
        {
            Weight sut = Weight.FromGram(input);

            Weight result = Weight.ConvertToPound(sut);

            result.Value.Should().BeApproximately(expectedValue, UnitConvertionConst.ApproxaimationValue);

            result.Unit.Should().Be(WeightUnit.Pound);
        }

        [TestCase(2.205 , 1)]
        [TestCase(1.323, .6)]
        [TestCase(6.934, 3.145)]
        [TestCase(45.34, 20.5663)]
        public void Should_convert_weight_from_pound_to_kilogram_unit(double input, double expectedValue)
        {
            Weight sut = Weight.FromPound(input);

            Weight result = Weight.ConvertToKiloGram(sut);

            result.Value.Should().BeApproximately(expectedValue, UnitConvertionConst.ApproxaimationValue);

            result.Unit.Should().Be(WeightUnit.KiloGram);
        }

        [TestCase(1, .001)]
        [TestCase(.1, .0001)]
        [TestCase(2000, 2)]
        [TestCase(500, .5)]
        public void Should_convert_weight_from_gram_to_kilogram_unit(double input , double expectedValue)
        {
            Weight sut = Weight.FromGram(input);

            Weight result = Weight.ConvertToKiloGram(sut);

            result.Value.Should().BeApproximately(expectedValue, UnitConvertionConst.ApproxaimationValue);

            result.Unit.Should().Be(WeightUnit.KiloGram);
        }

        [TestCase(5,5,true)]
        [TestCase(5,3,false)]
        [TestCase(1,3,false)]
        public void Should_return_correct_result_from_equlity_operator(double arg0, double arg1 , bool expectedResult)
        {
            Weight left = Weight.FromKiloGram(arg0);
            Weight right = Weight.FromKiloGram(arg1);

            bool result = left == right;

            result.Should().Be(expectedResult);          
        }



        [TestCase(5, 5, 10)]
        [TestCase(5, 3, 8)]
        [TestCase(1, 3, 4)]
        public void Should_return_correct_result_from_addition_operator(double arg0, double arg1, double expectedResult)
        {
            Weight left = Weight.FromKiloGram(arg0);
            Weight right = Weight.FromKiloGram(arg1);

            Weight result = left + right;

            result.Value.Should().Be(expectedResult);

            result.Unit.Should().Be(WeightUnit.KiloGram);
        }

        [TestCase(5, 5, 0)]
        [TestCase(5, 3, 2)]
        [TestCase(7, 3, 4)]
        public void Should_return_correct_result_from_subtract_operator(double arg0, double arg1, double expectedResult)
        {
            Weight left = Weight.FromKiloGram(arg0);
            Weight right = Weight.FromKiloGram(arg1);

            Weight result = left - right;

            result.Value.Should().Be(expectedResult);

            result.Unit.Should().Be(WeightUnit.KiloGram);
        }


       

    }
}
