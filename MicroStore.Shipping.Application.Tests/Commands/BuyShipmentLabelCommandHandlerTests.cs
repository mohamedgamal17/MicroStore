using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Tests.Fakes;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Domain.ValueObjects;
using System.Net;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Shipping.Application.Tests.Commands
{
    public class BuyShipmentLabelCommandHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_buy_shipment_label()
        {
            var fakeShipment=  await CreateFakeFullfilledShipment();

            var command = new BuyShipmentLabelCommand
            {
                SystemName = FakeConst.ActiveSystem,
                ExternalShipmentId = fakeShipment.ShipmentExternalId,
                RateId = Guid.NewGuid().ToString()
            };

            var result = await Send(command);



            var shipment = await RetriveShipment(result.GetEnvelopeResult<ShipmentDto>().Result.ShipmentId);

            shipment.Should().NotBeNull();

            shipment?.TrackingNumber?.Should().Be(result.GetEnvelopeResult<ShipmentDto>().Result.TrackingNumber);

            shipment?.Status.Should().Be(ShipmentStatus.Shipping);

            shipment?.ShipmentLabelExternalId.Should().Be(result.GetEnvelopeResult<ShipmentDto>().Result.ShipmentLabelExternalId);
        }

        [Test]
        public async Task Should_return_error_result_with_status_code_404_while_shipment_system_provider_is_not_exist()
        {
            var command = new BuyShipmentLabelCommand
            {
                SystemName = "NAN",
                ExternalShipmentId = Guid.NewGuid().ToString(),
                RateId = Guid.NewGuid().ToString()
            };

            var result=  await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }


        [Test]

        public async Task Should_return_error_result_with_status_code_401_while_shipment_system_provider_is_not_active()
        {

            var command = new BuyShipmentLabelCommand
            {
                SystemName = FakeConst.NotActiveSystem,
                ExternalShipmentId = Guid.NewGuid().ToString(),
                RateId = Guid.NewGuid().ToString()
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();


            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }


        private Task<Shipment> CreateFakeFullfilledShipment()
        {
            return WithUnitOfWork((sp) =>
            {
                var respository = sp.GetRequiredService<IShipmentRepository>();
                return respository.InsertAsync(new Shipment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Address.Empty)
                {
                    Status  = ShipmentStatus.Fullfilled,
                    ShipmentExternalId= Guid.NewGuid().ToString(),
                });
            });
        }

        private Task<Shipment?> RetriveShipment(Guid id)
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IShipmentRepository>();

                return repository.RetriveShipment(id, CancellationToken.None);
            });
        }

    }
}
