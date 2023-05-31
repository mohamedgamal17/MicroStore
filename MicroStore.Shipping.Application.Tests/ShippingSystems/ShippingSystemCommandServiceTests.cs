using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.ShippingSystems;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Shipping.Application.Tests.ShippingSystems
{
    public class ShippingSystemCommandServiceTests : BaseTestFixture
    {
        private readonly IShippingSystemCommandService _shippingSystemCommandService;

        public ShippingSystemCommandServiceTests()
        {
            _shippingSystemCommandService = GetRequiredService<IShippingSystemCommandService>();
        }

        [Test]
        public async Task Should_update_shipping_system()
        {
            var systemName = Guid.NewGuid().ToString();

            await InsertShippingSystem(systemName);

            var result = await _shippingSystemCommandService.EnableAsync(systemName, true);

            result.IsSuccess.Should().BeTrue();

            var system = await SingleAsync<ShippingSystem>( x=> x.Name == systemName);

            system.IsEnabled.Should().BeTrue();

        }

        [Test]
        public async Task Should_return_failure_result_while_enabling_shipping_system_when_shipping_system_is_not_exist()
        {
            var result = await _shippingSystemCommandService.EnableAsync(Guid.NewGuid().ToString(), true);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }


        private Task InsertShippingSystem(string name)
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<ShippingSystem>>();
                return repository.InsertAsync(new ShippingSystem
                {
                    DisplayName = name,
                    Name = name,
                    Image = "NAN",
                    IsEnabled = false
                });
            });
        }

    }
}
