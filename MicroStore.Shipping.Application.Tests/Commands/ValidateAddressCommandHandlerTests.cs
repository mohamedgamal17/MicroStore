using FluentAssertions;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Tests.Fakes;
using MicroStore.Shipping.Domain.Entities;
using System.Net;

namespace MicroStore.Shipping.Application.Tests.Commands
{
    public class When_address_is_valid_and_default_shipping_system_is_configured : BaseTestFixture
    {
        [Test]
        public async Task Should_validate_address_and_return_200_status_code()
        {
            var command = ValidateAddressUtilities.PrepareValidateAddressCommand();

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.IsSuccess.Should().BeTrue();
        }

        
    }

    public class When_address_is_valid_and_shipping_system_is_not_active : BaseTestFixture
    {
        [Test]
        public async Task Should_return_400_status_code()
        {
            var settings = new ShippingSettings()
            {
                DefaultShippingSystem = FakeConst.NotActiveSystem
            };

            await UpdateSettings(settings);

            var command = ValidateAddressUtilities.PrepareValidateAddressCommand();

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            result.IsFailure.Should().BeTrue();
        }

    }

    public class when_address_is_valid_and_shipping_system_is_not_configured : BaseTestFixture
    {
        [Test]
        public async Task Should_return_400_status_code_()
        {
            var settings = new ShippingSettings();

            await UpdateSettings(settings);

            var command = ValidateAddressUtilities.PrepareValidateAddressCommand();

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            result.IsFailure.Should().BeTrue();
        }

    }

    public class When_address_is_valid_and_shipping_system_is_not_exist : BaseTestFixture
    {


        [Test]
        public async Task Should_return_404_status_code()
        {
            var settings = new ShippingSettings
            {
                DefaultShippingSystem = "NONEXIST"
            };

            await UpdateSettings(settings);

            var command = ValidateAddressUtilities.PrepareValidateAddressCommand();

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            result.IsFailure.Should().BeTrue();
        }

    }

    internal static class ValidateAddressUtilities
    {
        public static ValidateAddressCommand PrepareValidateAddressCommand()
        {
            return new ValidateAddressCommand
            {
                CountryCode = "US",
                State = "CA",
                City = "San Jose",
                AddressLine1 = "525 S Winchester Blvd",
                AddressLine2 = "525 S Winchester Blvd",
                Name = "Jane Doe",
                Phone = "555-555-5555",
                PostalCode = "95128",
                Zip = "90241"
            };
        }
    }
   
}
