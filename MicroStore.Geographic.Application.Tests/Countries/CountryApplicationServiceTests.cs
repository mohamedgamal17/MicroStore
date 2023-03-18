using FluentAssertions;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Geographic.Application.Countries;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Models;
using MicroStore.Geographic.Application.Tests.Extensions;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Geographic.Application.Tests.Countries
{
    public class CountryApplicationServiceTests : BaseTestFixutre
    {
        private readonly ICountryApplicationService _countryApplicationService;

        public CountryApplicationServiceTests()
        {
            _countryApplicationService= GetRequiredService<ICountryApplicationService>();
        }

        [Test]
        public async Task Should_create_country()
        {
            var model = PrepareCountryModel();

            var result = await _countryApplicationService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var country = await Find<Country>(x => x.Id == result.Value.Id);

            country.AssertCountryModel(model);

            country.AssertCountryDto(result.Value);
        }

   

        [Test]
        public async Task Should_update_country()
        {
            var fakeCountry = await CreateCountry();

            var model = PrepareCountryModel();

            var result = await _countryApplicationService.UpdateAsync(fakeCountry.Id, model);

            var country = await Find<Country>(x => x.Id == result.Value.Id);

            result.IsSuccess.Should().BeTrue();

            country.AssertCountryModel(model);

            country.AssertCountryDto(result.Value);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_country_when_country_is_not_exist()
        {
            var model = PrepareCountryModel();

            var result = await _countryApplicationService.UpdateAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_delete_country()
        {
            var fakeCountry = await CreateCountry();

            var result = await _countryApplicationService.DeleteAsync(fakeCountry.Id);

            var country = await SingleOrDefaultAsync<Country>(x=> x.Id == fakeCountry.Id);

            country.Should().BeNull();
        }

        [Test]
        public async Task Should_return_failure_result_while_deleting_country_when_country_is_not_exist()
        {

            var result = await _countryApplicationService.DeleteAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_return_country_list()
        {
            await CreateCountry(); 

            var result = await _countryApplicationService.ListAsync();

            result.Count.Should().BeGreaterThan(0);

        }

        [Test]
        public async Task Should_get_country_by_id()
        {
            var fakeCountry =  await CreateCountry();

            var result = await _countryApplicationService.GetAsync(fakeCountry.Id);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(fakeCountry.Id);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_by_id_when_country_is_not_exist()
        {
            var result = await _countryApplicationService.GetAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_get_country_by_code()
        {
            var fakeCountry = await CreateCountry();

            var result = await _countryApplicationService.GetByCodeAsync(fakeCountry.TwoLetterIsoCode);

            result.IsSuccess.Should().BeTrue();

            result.Value.TwoLetterIsoCode.Should().Be(fakeCountry.TwoLetterIsoCode);
            result.Value.ThreeLetterIsoCode.Should().Be(fakeCountry.ThreeLetterIsoCode);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_by_code_when_country_is_not_exist()
        {
            var result = await _countryApplicationService.GetByCodeAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }
        protected CountryModel PrepareCountryModel()
        {
            return new CountryModel
            {
                Name = Guid.NewGuid().ToString(),
                NumericIsoCode = 22,
                TwoLetterIsoCode = RandomString(2),
                ThreeLetterIsoCode = RandomString(3),
            };
        }
    }
}
