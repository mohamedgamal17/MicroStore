using FluentAssertions;
using MicroStore.Profiling.Application.Domain;
using MicroStore.Profiling.Application.Models;
using MicroStore.Profiling.Application.Services;
using MicroStore.Profiling.Application.Tests.Extensions;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Profiling.Application.Tests
{
    public class ProfileCommandServiceTests : BaseTestFixture
    {
        private readonly IProfileCommandService _sut;

        public ProfileCommandServiceTests()
        {
            _sut = GetRequiredService<IProfileCommandService>();
        }

        [Test]
        public async Task Should_create_user_profile()
        {
            var model = GenerateCreateProfileModel();

            var result = await _sut.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var profile = await SingleAsync<Profile>(x => x.Id == result.Value.Id);

            profile.AssertProfile(model);

            profile.UserId.Should().Be(model.UserId);
        }

        [Test]
        public async Task Should_update_user_profile()
        {
            var fakeProfile = await CreateFakeProfile();

            var model = GenerateProfileModel();

            var result = await _sut.UpdateAsync(fakeProfile.UserId, model);

            result.IsSuccess.Should().BeTrue();

            var profile = await SingleAsync<Profile>(x => x.Id == result.Value.Id);

            profile.AssertProfile(model);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_profile_when_user_dose_not_have_profile()
        {
            var userId = Guid.NewGuid().ToString();

            var model = GenerateProfileModel();

            var result = await _sut.UpdateAsync(userId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_create_address()
        {
            var fakeProfile = await CreateFakeProfile();

            var model = GenerateAddressModel();

            var result = await _sut.CreateAddressAsync(fakeProfile.UserId, model);

            result.IsSuccess.Should().BeTrue();

            var profile = await SingleAsync<Profile>(x => x.Id == fakeProfile.Id);

            var address = profile.Addresses.Single(x => x.Id == result.Value.Id);

            address.AssertAddress(model);
        }

        [Test]
        public async Task Should_return_failure_result_while_creating_address_when_user_dose_not_have_profile()
        {
            var userId = Guid.NewGuid().ToString();

            var model = GenerateAddressModel();

            var result = await _sut.CreateAddressAsync(userId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_update_address()
        {
            var fakeProfile = await CreateFakeProfile();

            var model = GenerateAddressModel();

            var addressId = fakeProfile.Addresses.First().Id;

            var result = await _sut.UpdateAddressAsync(fakeProfile.UserId, addressId, model);

            result.IsSuccess.Should().BeTrue();

            var profile = await SingleAsync<Profile>(x => x.Id == fakeProfile.Id);

            var address = profile.Addresses.Single(x => x.Id == result.Value.Id);

            address.AssertAddress(model);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_address_when_user_dose_not_have_profile()
        {
            var userId = Guid.NewGuid().ToString();

            var model = GenerateAddressModel();

            var addressId = Guid.NewGuid().ToString();

            var result = await _sut.UpdateAddressAsync(userId, addressId , model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_return_failure_result_while_updating_address_when_address_is_not_exist()
        {
            var fakeProfile = await CreateFakeProfile();

            var model = GenerateAddressModel();

            var addressId = Guid.NewGuid().ToString();

            var result = await _sut.UpdateAddressAsync(fakeProfile.UserId, addressId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_remove_address()
        {
            var fakeProfile = await CreateFakeProfile();

            var addressId = fakeProfile.Addresses.First().Id;

            var result = await _sut.RemoveAddressAsync(fakeProfile.UserId, addressId);

            result.IsSuccess.Should().BeTrue();

            var profile = await SingleAsync<Profile>(x => x.Id == fakeProfile.Id);

            var address = profile.Addresses.SingleOrDefault(x => x.Id == addressId);

            address.Should().BeNull();

        }



        [Test]
        public async Task Should_return_failure_result_while_removing_address_when_user_dose_not_have_profile()
        {
            var userId = Guid.NewGuid().ToString();

            var addressId = Guid.NewGuid().ToString();

            var result = await _sut.RemoveAddressAsync(userId, addressId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_return_failure_result_while_removing_address_when_address_is_not_exist()
        {
            var fakeProfile = await CreateFakeProfile();

            var model = GenerateAddressModel();

            var addressId = Guid.NewGuid().ToString();

            var result = await _sut.RemoveAddressAsync(fakeProfile.UserId, addressId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        private ProfileModel GenerateProfileModel()
        {
            return new ProfileModel
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Avatar = Guid.NewGuid().ToString(),
                BirthDate = DateTime.Now.AddYears(-25),
                Gender = "female",
                Phone = "+18143511258",
                Addresses = new List<AddressModel>
                {
                    new AddressModel
                    {
                        Name = Guid.NewGuid().ToString(),
                        CountryCode = "US",
                        State = "TX",
                        City = "Texas",
                        Phone = "+18143511258",
                        AddressLine1 = Guid.NewGuid().ToString(),
                        AddressLine2 = Guid.NewGuid().ToString(),
                        PostalCode = "5432",
                        Zip = "632"

                    }
                }
            };
        }
        private CreateProfileModel GenerateCreateProfileModel()
        {
            return new CreateProfileModel
            {
                UserId = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Avatar = Guid.NewGuid().ToString(),
                BirthDate = DateTime.Now.AddYears(-25),
                Gender = "female",
                Phone = "+15852826903",
                Addresses = new List<AddressModel>
                {
                    new AddressModel
                    {
                        Name = Guid.NewGuid().ToString(),
                        CountryCode = "US",
                        State = "TX",
                        City = "Texas",
                        Phone = "+18143008176",
                        AddressLine1 = Guid.NewGuid().ToString(),
                        AddressLine2 = Guid.NewGuid().ToString(),
                        PostalCode = "5432",
                        Zip = "632"

                    }
                }
            };
        }


        private AddressModel GenerateAddressModel()
        {
            return new AddressModel
            {
                Name = Guid.NewGuid().ToString(),
                CountryCode = "US",
                State = "TX",
                City = "Texas",
                Phone = "+18143511258",
                AddressLine1 = Guid.NewGuid().ToString(),
                AddressLine2 = Guid.NewGuid().ToString(),
                PostalCode = "3215",
                Zip = "2315"

            };
        }
    }

}