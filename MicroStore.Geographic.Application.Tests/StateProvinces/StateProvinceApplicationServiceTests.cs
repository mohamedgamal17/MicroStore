using FluentAssertions;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Models;
using MicroStore.Geographic.Application.StateProvinces;
using MicroStore.Geographic.Application.Tests.Extensions;
using Volo.Abp.Domain.Entities;

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

            var country = await SingleAsync<Country>(x=> x.Id == fakeCountry.Id, x=> x.StateProvinces);
            var stateProvince = await SingleAsync<StateProvince>(x => x.Id == result.Value.Id);

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

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_update_state_province()
        {
            var fakeCountry = await CreateCountryWithStateProvince();

            var model = PrepareStateProvinceModel();

            var result = await _stateProvinceApplicationService.UpdateAsync(fakeCountry.Id, fakeCountry.StateProvinces.First().Id, model);

            result.IsSuccess.Should().BeTrue();

            var stateProvince = await SingleAsync<StateProvince>(x => x.Id == fakeCountry.StateProvinces.First().Id);

            stateProvince.AssertStateProvinceModel(model);
            stateProvince.AssertStateProvinceDto(result.Value);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_state_province_when_country_is_not_exist()
        {
            var model = PrepareStateProvinceModel();

            var result = await _stateProvinceApplicationService.UpdateAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();


            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_return_failure_result_while_updating_state_province_when_state_province_is_not_exist_in_country()
        {
            var fakeCountry = await CreateCountry();

            var model = PrepareStateProvinceModel();

            var result = await _stateProvinceApplicationService.UpdateAsync(fakeCountry.Id, Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();


            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_delete_state_province()
        {
            var fakeCountry = await CreateCountryWithStateProvince();

            var state = await FirstAsync<StateProvince>();

            var stateId = fakeCountry.StateProvinces.First().Id;

            var result = await _stateProvinceApplicationService.DeleteAsync(fakeCountry.Id, stateId);

            result.IsSuccess.Should().BeTrue();

            var country = await SingleAsync<Country>(x => x.Id == fakeCountry.Id, prop=> prop.StateProvinces);

            country.StateProvinces.Where(x => x.Id == stateId).SingleOrDefault().Should().BeNull();

        }


        [Test]
        public async Task Should_return_failure_result_while_deleting_state_province_when_country_is_not_exist()
        {
  
            var result = await _stateProvinceApplicationService.DeleteAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();


            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_return_failure_result_while_deleting_state_province_when_state_province_is_not_exist_in_country()
        {
            var fakeCountry = await CreateCountry();

            var result = await _stateProvinceApplicationService.DeleteAsync(fakeCountry.Id, Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

                
            result.Exception.Should().BeOfType<EntityNotFoundException>();

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
