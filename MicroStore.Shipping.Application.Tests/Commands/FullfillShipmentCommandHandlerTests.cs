using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Tests.Fakes;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Domain.ValueObjects;
using System.Net;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Shipping.Application.Tests.Commands
{
    public class FullfillShipmentCommandHandlerTests : BaseTestFixture
    {

        [Test]
        public async Task Should_fullfill_shipment()
        {
            var fakeShipment = await CreateFakeShipment();

            var command = PrepareFullfillShipmentCommand(fakeShipment);

            var result = await Send(command);

            var shipment = await RetriveShipment(fakeShipment.Id);

            shipment.Should().NotBeNull();

            shipment?.ShipmentExternalId.Should().Be(result.GetEnvelopeResult<ShipmentFullfilledDto>().Result.ExternalShipmentId);

            shipment?.Status.Should().Be(ShipmentStatus.Fullfilled);

        }

        [Test]
        public async Task Should_return_error_result_with_status_code_404_while_shipment_is_not_exist()
        {
            var command = PrepareFullfillShipmentCommand(
                new Shipment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Address.Empty)
             );

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Should_return_error_result_with_status_code_404_while_shipment_system_provider_is_not_exist() 
        {
            var fakeShipment = await CreateFakeShipment();

            var command = PrepareFullfillShipmentCommand(fakeShipment);

            command.SystemName = "NAN";

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }


        [Test]
        public async Task Should_return_error_result_with_status_code_400_while_shipment_system_provider_is_not_exist()
        {
            var fakeShipment = await CreateFakeShipment();

            var command = PrepareFullfillShipmentCommand(fakeShipment);

            command.SystemName = FakeConst.NotActiveSystem;

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        }




        private FullfillShipmentCommand PrepareFullfillShipmentCommand(Shipment fakeShipment)
        {
            return new FullfillShipmentCommand
            {
                ShipmentId = fakeShipment.Id,
                SystemName = FakeConst.ActiveSystem,
                AddressFrom = new AddressModel
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
                },
                Pacakge = new PackageModel
                {
                    Dimension = new DimensionModel
                    {
                        Height = 5,
                        Width = 5,
                        Lenght = 5,
                        Unit = StandardDimensionUnit.Inch
                    },

                    Weight = new WeightModel
                    {
                        Value = 5,
                        Unit = StandardWeightUnit.Gram
                    }
                }
            };
        }
        private Task<Shipment> CreateFakeShipment()
        {
            return WithUnitOfWork((sp) =>
            {
                var respository = sp.GetRequiredService<IShipmentRepository>();
                return respository.InsertAsync(new Shipment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Address.Empty));
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
