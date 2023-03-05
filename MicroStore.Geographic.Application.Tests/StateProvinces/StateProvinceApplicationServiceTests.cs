using FluentAssertions;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Models;
using MicroStore.Geographic.Application.StateProvinces;
using MicroStore.Geographic.Application.Tests.Extensions;

namespace MicroStore.Geographic.Application.Tests.StateProvinces
{
    public class StateProvinceApplicationServiceTests : BaseTestFixutre
    {
        private readonly IStateProvinceApplicationService _stateProvinceApplicationService;

        public StateProvinceApplicationServiceTests()
        {
            _stateProvinceApplicationService = GetRequiredService<IStateProvinceApplicationService>();
        }

        [Test]
        public async Task Should_create_state_province()
        {
            var fakeCountry = await CreateCountry();

            var model = PrepareStateProvinceModel();

            var result = await _stateProvinceApplicationService.CreateAsync(fakeCountry.Id, model);

            result.IsSuccess.Should().BeTrue();

            var country = await Find<Country>(x=> x.Id == fakeCountry.Id, x=> x.StateProvinces);
            var stateProvince = await Find<StateProvince>(x => x.Id == result.Value.Id);

            country.StateProvinces.Count.Should().Be(1);

            stateProvince.AssertStateProvinceModel(model);
            stateProvince.AssertStateProvinceDto(result.Value);

        }

        [Test]
        public async Task Should_return_failure_result_while_creating_state_province_when_country_is_not_exist()
        {
            var model = PrepareStateProvinceModel();

            var result = await _stateProvinceApplicationService.CreateAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }

        [Test]
        public async Task Should_update_state_province()
        {
            var fakeCountry = await CreateCountryWithStateProvince();

            var model = PrepareStateProvinceModel();

            var result = await _stateProvinceApplicationService.UpdateAsync(fakeCountry.Id, fakeCountry.StateProvinces.First().Id, model);

            result.IsSuccess.Should().BeTrue();

            var stateProvince = await Find<StateProvince>(x => x.Id == fakeCountry.StateProvinces.First().Id);

            stateProvince.AssertStateProvinceModel(model);
            stateProvince.AssertStateProvinceDto(result.Value);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_state_province_when_country_is_not_exist()
        {
            var model = PrepareStateProvinceModel();

            var result = await _stateProvinceApplicationService.UpdateAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();


            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);

        }

        [Test]
        public async Task Should_return_failure_result_while_updating_state_province_when_state_province_is_not_exist_in_country()
        {
            var fakeCountry = await CreateCountry();

            var model = PrepareStateProvinceModel();

            var result = await _stateProvinceApplicationService.UpdateAsync(fakeCountry.Id, Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();


            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);

        }

        [Test]
        public async Task Should_delete_state_province()
        {
            var fakeCountry = await CreateCountryWithStateProvince();

            var state = await FirstAsync<StateProvince>();

            var stateId = fakeCountry.StateProvinces.First().Id;

            var result = await _stateProvinceApplicationService.DeleteAsync(fakeCountry.Id, stateId);

            result.IsSuccess.Should().BeTrue();

            var country = await Find<Country>(x => x.Id == fakeCountry.Id, prop=> prop.StateProvinces);

            country.StateProvinces.Where(x => x.Id == stateId).SingleOrDefault().Should().BeNull();

        }


        [Test]
        public async Task Should_return_failure_result_while_deleting_state_province_when_country_is_not_exist()
        {
  
            var result = await _stateProvinceApplicationService.DeleteAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();


            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);

        }

        [Test]
        public async Task Should_return_failure_result_while_deleting_state_province_when_state_province_is_not_exist_in_country()
        {
            var fakeCountry = await CreateCountry();

            var result = await _stateProvinceApplicationService.DeleteAsync(fakeCountry.Id, Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();


            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);

        }

        [Test]
        public async Task Should_list_country_state_provinces()
        {
            var fakeCountry = await CreateCountryWithStateProvince();

            var result = await _stateProvinceApplicationService.ListAsync(fakeCountry.Id);

            result.IsSuccess.Should().BeTrue();

            result.Value.All(x => x.CountryId == fakeCountry.Id);
        }


        [Test]
        public async Task Should_return_failure_result_while_listing_country_state_provinces_when_country_not_exist()
        {
            var result = await _stateProvinceApplicationService.ListAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }



        [Test]
        public async Task Should_get_country_state_province()
        {
            var fakeCountry = await CreateCountryWithStateProvince();
            var stateId = fakeCountry.StateProvinces.First().Id;
            var result = await _stateProvinceApplicationService.GetAsync(fakeCountry.Id,stateId);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(stateId);
            result.Value.CountryId.Should().Be(fakeCountry.Id);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_country_state_province_when_country_not_exist()
        {
            var result = await _stateProvinceApplicationService.GetAsync(Guid.NewGuid().ToString(),Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }


        [Test]
        public async Task Should_return_failure_result_while_getting_country_state_province_when_state_province_is_not_exist_is_country()
        {
            var fakeCountry = await CreateCountry();

            var result = await _stateProvinceApplicationService.GetAsync(fakeCountry.Id, Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }




        [Test]
        public async Task Should_get_country_state_by_code_province()
        {
            var fakeCountry = await CreateCountryWithStateProvince();
            var stateCode = fakeCountry.StateProvinces.First().Abbreviation;
            var result = await _stateProvinceApplicationService.GetByCodeAsync(fakeCountry.TwoLetterIsoCode, stateCode);

            result.IsSuccess.Should().BeTrue();

            result.Value.Abbreviation.Should().Be(stateCode);
            result.Value.CountryId.Should().Be(fakeCountry.Id);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_country_state_province_by_code_when_country_not_exist()
        {
            var result = await _stateProvinceApplicationService.GetByCodeAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }


        [Test]
        public async Task Should_return_failure_result_while_getting_country_state_province_by_code_when_state_province_is_not_exist_is_country()
        {
            var fakeCountry = await CreateCountry();

            var result = await _stateProvinceApplicationService.GetByCodeAsync(fakeCountry.TwoLetterIsoCode, Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }
        protected StateProvinceModel PrepareStateProvinceModel()
        {
            return new StateProvinceModel
            {
                Name = Guid.NewGuid().ToString(),
                Abbreviation = RandomString(2)
            };
        }
    }
}
