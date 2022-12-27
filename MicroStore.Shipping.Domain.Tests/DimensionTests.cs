//using FluentAssertions;
//using MicroStore.Shipping.Domain.ValueObjects;

//namespace MicroStore.Shipping.Domain.Tests
//{
//    [TestFixture]
//    public class DimensionTests
//    {
//        [TestCase(2.2, 3 ,.5 ,220,300,50)]
//        [TestCase(.5, .8 ,1 ,50,80,100)]
//        [TestCase(5, 10 ,15 ,500,1000,1500)]
//        public void Should_convert_dimension_from_meter_to_centimeter_unit(double width , double lenght , double height , double expectedWidth , double expectedLenght ,  double expectedHeight)
//        {
//            Dimension sut = Dimension.FromMeter(width, lenght , height);

//            Dimension result =  Dimension.ConvertToCentiMeter(sut);

//            result.Width.Should().BeApproximately(expectedWidth , UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght , UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

            
//        }

//        [TestCase(.5, .8 , .6, 15.24, 24.384, 18.288)]
//        [TestCase(1.5, 1.8 , 1.6, 45.72, 54.864, 48.768)]
//        [TestCase(5, 8 , 16, 152.4, 243.84, 487.68)]
//        public void Should_convert_dimension_from_feet_to_centimeter_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromFeet(width, lenght, height);

//            Dimension result = Dimension.ConvertToCentiMeter(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.CentiMeter);
//        }

//        [TestCase(.5, .8, .6, 1.27, 2.032, 1.524)]
//        [TestCase(1.5, 1.8, 1.6, 3.81, 4.572, 4.064)]
//        [TestCase(5, 8, 16, 12.7, 20.32, 40.64)]
//        public void Should_convert_dimension_from_inch_to_centimeter_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromInch(width, lenght, height);

//            Dimension result = Dimension.ConvertToCentiMeter(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.CentiMeter);
//        }

//        [TestCase(.5, .8, .6, 19.68503937, 31.496062992, 23.622047244)]
//        [TestCase(1.5, 1.8, 1.6, 59.05511811, 70.866141732, 62.992125984)]
//        [TestCase(5, 8, 16, 196.8503937, 314.96062992, 629.92125984)]
//        public void Should_convert_dimension_from_meter_to_inch_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromMeter(width, lenght, height);

//            Dimension result = Dimension.ConvertToInch(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.Inch);
//        }

//        [TestCase(.5, .8, .6, 6, 9.6, 7.2)]
//        [TestCase(1.5, 1.8, 1.6, 18, 21.6, 19.2)]
//        [TestCase(5, 8, 16, 60, 96, 192)]
//        public void Should_convert_dimension_from_feet_to_inch_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromFeet(width, lenght, height);

//            Dimension result = Dimension.ConvertToInch(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.Inch);
//        }

//        [TestCase(.5, .8, .6, 0.1968503937, 0.31496062992, 0.23622047244)]
//        [TestCase(1.5, 1.8, 1.6, 0.5905511811, 0.70866141732, 0.62992125984)]
//        [TestCase(5, 8, 16, 1.968503937, 3.1496062992, 6.2992125984)]
//        public void Should_convert_dimension_from_centimeter_to_inch_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromCentiMeter(width, lenght, height);

//            Dimension result = Dimension.ConvertToInch(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.Inch);
//        }


//        [TestCase(.5, .8, .6, 1.6404199475, 2.624671916, 1.968503937)]
//        [TestCase(1.5, 1.8, 1.6, 4.9212598425, 5.905511811, 5.249343832)]
//        [TestCase(5, 8, 16, 16.404199475, 26.24671916, 52.49343832)]
//        public void Should_convert_dimension_from_meter_to_feet_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromMeter(width, lenght, height);

//            Dimension result = Dimension.ConvertToFeet(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.Feet);
//        }

//        [TestCase(.5, .8, .6, 0.041666666667, 0.066666666667, 0.05)]
//        [TestCase(1.5, 1.8, 1.6, 0.125, 0.15, 0.13333333333)]
//        [TestCase(5, 8, 16, 0.41666666667, 0.66666666667, 1.3333333333)]
//        public void Should_convert_dimension_from_inch_to_feet_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromInch(width, lenght, height);

//            Dimension result = Dimension.ConvertToFeet(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.Feet);
//        }

//        [TestCase(.5, .8, .6, 0.016404199475, 0.02624671916, 0.01968503937)]
//        [TestCase(1.5, 1.8, 1.6, 0.049212598425, 0.05905511811, 0.05249343832)]
//        [TestCase(5, 8, 16, 0.16404199475, 0.2624671916, 0.5249343832)]
//        public void Should_convert_dimension_from_centimeter_to_feet_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromCentiMeter(width, lenght, height);

//            Dimension result = Dimension.ConvertToFeet(sut);

//            result.Width.Should().BeApproximately(expectedWidth,0.1F);

//            result.Length.Should().BeApproximately(expectedLenght, 0.1F);

//            result.Height.Should().BeApproximately(expectedHeight, 0.01F);

//            result.Unit.Should().Be(DimensionUnit.Feet);
//        }



//        [TestCase(.5, .8, .6, 0.1524, 0.24384, 0.18288)]
//        [TestCase(1.5, 1.8, 1.6, 0.4572, 0.54864, 0.48768)]
//        [TestCase(5, 8, 16, 1.524, 2.4384, 4.8768)]
//        public void Should_convert_dimension_from_feet_to_meter_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromFeet(width, lenght, height);

//            Dimension result = Dimension.ConvertToMeter(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.Meter);
//        }

//        [TestCase(.5, .8, .6, 0.0127, 0.02032, 0.01524)]
//        [TestCase(1.5, 1.8, 1.6, 0.0381, 0.04572, 0.04064)]
//        [TestCase(5, 8, 16, 0.127, 0.2032, 0.4064)]
//        public void Should_convert_dimension_from_inch_to_meter_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromInch(width, lenght, height);

//            Dimension result = Dimension.ConvertToMeter(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.Meter);
//        }

//        [TestCase(.5, .8, .6, .005, .008, .006)]
//        [TestCase(1.5, 1.8, 1.6, .015,.018,.016)]
//        [TestCase(5, 8, 16, .05, .08, .16)]
//        public void Should_convert_dimension_from_centimeter_to_meter_unit(double width, double lenght, double height, double expectedWidth, double expectedLenght, double expectedHeight)
//        {
//            Dimension sut = Dimension.FromCentiMeter(width, lenght, height);

//            Dimension result = Dimension.ConvertToMeter(sut);

//            result.Width.Should().BeApproximately(expectedWidth, UnitConvertionConst.ApproxaimationValue);

//            result.Length.Should().BeApproximately(expectedLenght, UnitConvertionConst.ApproxaimationValue);

//            result.Height.Should().BeApproximately(expectedHeight, UnitConvertionConst.ApproxaimationValue);

//            result.Unit.Should().Be(DimensionUnit.Meter);
//        }




//    }
//}
