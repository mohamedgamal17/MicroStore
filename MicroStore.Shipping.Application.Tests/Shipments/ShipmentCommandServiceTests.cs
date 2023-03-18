using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Shipments;
using MicroStore.Shipping.Application.Tests.Fakes;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Domain.ValueObjects;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Shipping.Application.Tests.Shipments
{

    public class ShipmentCommandServiceTests : ShipmentCommandServiceTestBase
    {

        private readonly IShipmentCommandService _shipmentCommandService;

        public ShipmentCommandServiceTests()
        {
            _shipmentCommandService = GetRequiredService<IShipmentCommandService>();
        }



        [Test]
        public async Task Should_create_Shipment()
        {
            var model = GenerateShipmentModel();

            var result = await _shipmentCommandService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();


            var shipment = await RetriveShipment(result.Value.Id);

            var item = model.Items.First();

            var shipmentItem = shipment?.Items.FirstOrDefault();

            shipment.Should().NotBeNull();
            shipment?.OrderId.Should().Be(result.Value.OrderId);
            shipment?.OrderNumber.Should().Be(result.Value.OrderNumber);
            shipment?.UserId.Should().Be(result.Value.UserId);
            shipment?.Address.Should().Be(model.Address.AsAddress());
            shipmentItem.Should().NotBeNull();
            shipmentItem?.Name.Should().Be(item.Name);
            shipmentItem?.Sku.Should().Be(item.Sku);
            shipmentItem?.ProductId.Should().Be(item.ProductId);
            shipmentItem?.Thumbnail.Should().Be(item.Thumbnail);
            shipmentItem?.Dimension.Should().Be(item.Dimension.AsDimension());
            shipmentItem?.Weight.Should().Be(item.Weight.AsWeight());
        }



        [Test]
        public async Task Should_fullfill_shipment()
        {
            var fakeShipment = await CreateFakeShipment();

            var model = GeneratePackageModel();

            var result = await _shipmentCommandService.FullfillAsync(fakeShipment.Id, model);

            var shipment = await Find<Shipment>(x=> x.Id== fakeShipment.Id);

            shipment.Should().NotBeNull();

            shipment?.ShipmentExternalId.Should().Be(result.Value.ShipmentExternalId);

            shipment?.Status.Should().Be(ShipmentStatus.Fullfilled);

        }


        [Test]
        public async Task Should_return_error_result_while_fullfilling_shipment_when_shipment_is_not_exist()
        {
            var model = GeneratePackageModel();

            var result = await _shipmentCommandService.FullfillAsync(Guid.NewGuid().ToString(), model);


            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_buy_shipment_label()
        {
            var fakeShipment = await CreateFakeFullfilledShipment();

            var model = new BuyShipmentLabelModel
            {
                ShipmentRateId = Guid.NewGuid().ToString()
            };

            var result = await _shipmentCommandService.BuyLabelAsync(fakeShipment.Id, model);

            var shipment = await Find<Shipment>(x=> x.Id == result.Value.Id);

            shipment.Should().NotBeNull();

            shipment?.TrackingNumber?.Should().Be(result.Value.TrackingNumber);

            shipment?.Status.Should().Be(ShipmentStatus.Shipping);

            shipment?.ShipmentLabelExternalId.Should().Be(result.Value.ShipmentLabelExternalId);
        }


        [Test]

        public async Task Should_return_failure_result_while_buy_shipment_label_when_shipment_is_not_exist()
        {
            var model = new BuyShipmentLabelModel
            {
                ShipmentRateId = Guid.NewGuid().ToString()
            };

            var result = await _shipmentCommandService.BuyLabelAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        private Task<Shipment?> RetriveShipment(string id)
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IShipmentRepository>();

                return repository.RetriveShipment(id, CancellationToken.None);
            });
        }
    }
}
