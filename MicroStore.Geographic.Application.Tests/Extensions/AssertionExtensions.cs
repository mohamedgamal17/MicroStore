using FluentAssertions;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;

namespace MicroStore.Geographic.Application.Tests.Extensions
{
    public static class AssertionExtensions
    {

        public static void AssertCountryModel(this Country country , CountryModel model)
        {
            country.Name.Should().Be(model.Name);
            country.NumericIsoCode.Should().Be(model.NumericIsoCode);
            country.TwoLetterIsoCode.Should().Be(model.TwoLetterIsoCode);
            country.ThreeLetterIsoCode.Should().Be(model.ThreeLetterIsoCode);
        }

        public static void AssertCountryDto(this Country country , CountryDto countryDto)
        {
            country.Id.Should().Be(countryDto.Id);
            country.Name.Should().Be(countryDto.Name);
            country.NumericIsoCode.Should().Be(countryDto.NumericIsoCode);
            country.TwoLetterIsoCode.Should().Be(countryDto.TwoLetterIsoCode);
            country.ThreeLetterIsoCode.Should().Be(countryDto.ThreeLetterIsoCode);
        }

        public static void AssertStateProvinceModel(this StateProvince country , StateProvinceModel model)
        {
            country.Name.Should().Be(model.Name);
            country.Abbreviation.Should().Be(model.Abbreviation);
        }

        public static void AssertStateProvinceDto(this StateProvince stateProvince , StateProvinceDto stateProvinceDto)
        {
            stateProvince.Id.Should().Be(stateProvinceDto.Id);
            stateProvince.Name.Should().Be(stateProvinceDto.Name);
            stateProvince.Abbreviation.Should().Be(stateProvinceDto.Abbreviation);
        }
    }
}
