using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Shipping.Application.Tests.Commands
{
    public class UpdateShippingSystemCommandHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_update_shipping_system()
        {
            var systemName = Guid.NewGuid().ToString();

            await InsertShippingSystem(systemName);

            var command = new UpdateShippingSystemCommand
            {
                SystemName = systemName,
                IsEnabled = true
            };

            await Send(command);


            var system = await Get(systemName);

            system.IsEnabled.Should().BeTrue();

        }

        [Test]
        public async Task Should_return_error_result_with_status_code_404_while_shipping_system_is_not_exist()
        {
            var systemName = Guid.NewGuid().ToString();


            var command = new UpdateShippingSystemCommand
            {
                SystemName = systemName,
                IsEnabled = true
            };
            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }


        private  Task InsertShippingSystem(string name)
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<ShippingSystem>>();
                return repository.InsertAsync(new ShippingSystem
                {
                    DisplayName = name,
                    Name = name,
                    Image = "NAN",
                    IsEnabled  = false
                });
            });
        }


        private Task<ShippingSystem> Get(string name)
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<ShippingSystem>>();
                return repository.SingleAsync(x => x.Name == name);
            });
        }
    }
}
