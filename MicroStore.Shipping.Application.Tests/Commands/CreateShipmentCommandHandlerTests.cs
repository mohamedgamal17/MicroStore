using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Domain.ValueObjects;
using System.Net;
using Volo.Abp;
namespace MicroStore.Shipping.Application.Tests.Commands
{
    public class CreateShipmentCommandHandlerTests : BaseTestFixture
    {

        [Test]
        public async Task Should_create_Shipment()
        {
            var command = PrepareCreateShipmentCommand();

            var result = await Send(command);

            var shipment = await RetriveShipment(result.GetEnvelopeResult<ShipmentDto>().Result.ShipmentId);

            var item = command.Items.First();

            var shipmentItem = shipment?.Items.FirstOrDefault();

            shipment.Should().NotBeNull();
            shipment?.OrderId.Should().Be(result.GetEnvelopeResult<ShipmentDto>().Result.OrderId);
            shipment?.UserId.Should().Be(result.GetEnvelopeResult<ShipmentDto>().Result.UserId);
            shipment?.Address.Should().Be(command.Address.AsAddress());
            shipmentItem.Should().NotBeNull();
            shipmentItem?.Name.Should().Be(item.Name);
            shipmentItem?.Sku.Should().Be(item.Sku);
            shipmentItem?.ProductId.Should().Be(item.ProductId);
            shipmentItem?.Thumbnail.Should().Be(item.Thumbnail);
            shipmentItem?.Dimension.Should().Be(item.Dimension.AsDimension());
            shipmentItem?.Weight.Should().Be(item.Weight.AsWeight());
        }

        [Test]
        public async Task Should_return_error_result_with_status_code_400_while_order_shipment_is_already_created()
        {
            var fakeShipment = await CreateFakeShipment();

            var command = new CreateShipmentCommand
            {
                UserId = Guid.NewGuid().ToString(),
                OrderId = fakeShipment.OrderId,
                Address = new AddressModel
                {
                    CountryCode = Guid.NewGuid().ToString(),
                    City = Guid.NewGuid().ToString(),
                    State = Guid.NewGuid().ToString(),
                    PostalCode = Guid.NewGuid().ToString(),
                    Zip = Guid.NewGuid().ToString(),
                    Phone = Guid.NewGuid().ToString(),
                    AddressLine1 = Guid.NewGuid().ToString(),
                    AddressLine2 = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                },
                Items = new List<ShipmentItemModel>()
            };

            var result  =  await Send(command);

            result.IsFailure.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        } 


        private CreateShipmentCommand PrepareCreateShipmentCommand()
        {
            return new CreateShipmentCommand
            {
                OrderId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Address = new AddressModel
                {
                    CountryCode = Guid.NewGuid().ToString(),
                    City = Guid.NewGuid().ToString(),
                    State = Guid.NewGuid().ToString(),
                    PostalCode = Guid.NewGuid().ToString(),
                    Zip = Guid.NewGuid().ToString(),
                    Phone = Guid.NewGuid().ToString(),
                    AddressLine1 = Guid.NewGuid().ToString(),
                    AddressLine2 = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                },

                Items = new List<ShipmentItemModel>
                {
                    new ShipmentItemModel
                    {
                        Name = Guid.NewGuid().ToString(),
                        Sku  =Guid.NewGuid().ToString(),
                        ProductId= Guid.NewGuid().ToString(),
                        Thumbnail = Guid.NewGuid().ToString(),
                        Quantity = 5,
                        Dimension =new DimensionModel
                        {
                            Height = 5,
                            Width = 5,
                            Lenght = 5,
                            Unit = "inch"
                        },
                        Weight = new WeightModel
                        {
                            Value = 5,
                            Unit = "g",
                        },
                        UnitPrice = 50
                    }
                }

            };
        }

        private Task<Shipment?> RetriveShipment(Guid id)
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IShipmentRepository>();

                return repository.RetriveShipment(id, CancellationToken.None);
            });
        }

        private Task<Shipment> CreateFakeShipment()
        {
            return WithUnitOfWork((sp) =>
            {
                var respository = sp.GetRequiredService<IShipmentRepository>();
                return respository.InsertAsync(new Shipment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Address.Empty));
            });
        }
    }
}
