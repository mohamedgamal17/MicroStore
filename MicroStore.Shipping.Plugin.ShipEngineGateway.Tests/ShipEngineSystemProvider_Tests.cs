using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Settings;
using System.Net;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Tests
{
    public class ShipEngineSystemProvider_Tests : BaseTestFixture
    {

        [Test]
        public async Task Should_fullfill_shipmet()
        {
            var settings = await SettingsRepository.TryToGetSettings<ShipEngineSettings>(ShipEngineConst.SystemName);

            var fakeShipment = await CreateShipmentWithInitialStatus();

            var sut = ServiceProvider.GetRequiredService<ShipEngineSystemProvider>();

            var model =  new FullfillModel
            {
                Package = new PackageModel
                {
                    Dimension = new DimensionModel
                    {
                        Lenght = 24,
                        Width = 12,
                        Height = 6,
                        Unit = "inch"
                    },

                    Weight = new WeightModel
                    {
                        Value = 40,
                        Unit = "g"
                    }
                },
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

                }
            };

            var responseResult =  await sut.Fullfill(fakeShipment.Id, model);

            responseResult.StatusCode.Should().Be((int) HttpStatusCode.OK);

            var result = responseResult.GetEnvelopeResult<ShipmentDto>().Result;

            var shipment = await SingleAsync<Shipment>(x=> x.Id == result.Id);

            shipment.ShipmentExternalId.Should().Be(result.ShipmentExternalId);

            shipment.SystemName.Should().Be(ShipEngineConst.SystemName);

            shipment.Status.Should().Be(ShipmentStatus.Fullfilled);
        }

        [Test]
        public async Task Should_retrive_shipment_rates()
        {
            var fakeShipment = await CreateShipmentWithFullfillStatus();

            var sut = ServiceProvider.GetRequiredService<ShipEngineSystemProvider>();

            var responseResult = await sut.RetriveShipmentRates(fakeShipment.ShipmentExternalId!);

            responseResult.StatusCode.Should().Be((int )HttpStatusCode.OK);
            responseResult.IsSuccess.Should().BeTrue();

            var result = responseResult.GetEnvelopeResult<List<ShipmentRateDto>>().Result;

            result.Count.Should().NotBe(0);
            var rate = result.First();
            rate.Id.Should().NotBeNull();
            rate.CarrierId.Should().NotBeNull();
            rate.Amount.Should().NotBeNull();
            rate.Amount.Value.Should().NotBe(0);
            rate.Amount.Currency.Should().NotBeNull();
            rate.ServiceLevel.Should().NotBeNull();
            var serviceLevel = rate.ServiceLevel;
            serviceLevel.Name.Should().NotBeNull();
            serviceLevel.Code.Should().NotBeNull();
        }

        [Test]
        public async Task Should_buy_shipment_label()
        {
            var fakeShipment = await CreateShipmentWithFullfillStatus();

            var sut= ServiceProvider.GetRequiredService<ShipEngineSystemProvider>();

            var ratesResult = await sut.RetriveShipmentRates(fakeShipment.ShipmentExternalId!);

            var rates = ratesResult.GetEnvelopeResult<List<ShipmentRateDto>>().Result;

            var model = new BuyShipmentLabelModel
            {
                ShipmentRateId = rates.First().Id
            };

            var responseResult = await sut.BuyShipmentLabel(fakeShipment.ShipmentExternalId!, model);

            responseResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            responseResult.IsSuccess.Should().BeTrue();

            var result = responseResult.GetEnvelopeResult<ShipmentDto>().Result;

            var shipment = await SingleAsync<Shipment>(x => x.Id == fakeShipment.Id);

            shipment.ShipmentLabelExternalId.Should().NotBeNull();

            shipment.ShipmentLabelExternalId.Should().Be(result.ShipmentLabelExternalId);

            shipment.TrackingNumber.Should().Be(result.TrackingNumber);

            shipment.Status.Should().Be(ShipmentStatus.Shipping);
        }

        [Test]
        public async Task Should_return_address_valid_when_address_is_valid()
        {
            var address = new AddressModel
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

            var sut = ServiceProvider.GetRequiredService<ShipEngineSystemProvider>();

            var result = await sut.ValidateAddress(address);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            result.IsSuccess.Should().BeTrue();

            var envelopeResut = result.GetEnvelopeResult<AddressValidationResultModel>();

            envelopeResut?.Should().NotBeNull();

            envelopeResut!.Result.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_address_unvalid_when_address_is_not_valid()
        {
            var address = new AddressModel
            {
                CountryCode = "EG",
                State = "XE",
                City = "San Jose",
                AddressLine1 = "525 S Winchester Blvd",
                AddressLine2 = "525 S Winchester Blvd",
                Name = "Jane Doe",
                Phone = "555-555-5555",
                PostalCode = "95128",
                Zip = "90241"

            };


            var sut = ServiceProvider.GetRequiredService<ShipEngineSystemProvider>();

            var result = await sut.ValidateAddress(address);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            result.IsSuccess.Should().BeTrue();

            var envelopeResut = result.GetEnvelopeResult<AddressValidationResultModel>();

            envelopeResut?.Should().NotBeNull();

            envelopeResut!.Result.IsValid.Should().BeFalse();
        }


        [Test]
        public async Task Should_estimate_shipping_rate_price()
        {
            var sut = ServiceProvider.GetRequiredService<ShipEngineSystemProvider>();

            var model = PreapreEstaimtionModel();

            var result = await sut.EstimateShipmentRate(model);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var response = result.GetEnvelopeResult<List<EstimatedRateDto>>().Result;

            response.Count.Should().NotBe(0);

            var rate = response.First();

            rate.EstaimatedDays.Should().NotBe(0);
            rate.ShippingDate.Should().NotBeNull();
            rate.Money.Value.Should().NotBe(0);
            rate.Money.Currency.Should().NotBeNull();
        }

        private EstimatedRateModel PreapreEstaimtionModel()
        {
            return new EstimatedRateModel
            {
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

                AddressTo = new AddressModel
                {
                    Name = "john doe",
                    Phone = "555-555-5555",
                    CountryCode = "US",
                    State = "TX",
                    City = "Austin",
                    AddressLine1 = "525 S Winchester Blvd",
                    AddressLine2 = "525 S Winchester Blvd",
                    PostalCode = "75462",
                    Zip = "73301"
                },

                Items = new List<ShipmentItemEstimationModel>
                {
                    new ShipmentItemEstimationModel
                    {
                        Sku =Guid.NewGuid().ToString(),
                        Name = Guid.NewGuid().ToString(),
                        Quantity = 5,
                        UnitPrice = new MoneyDto
                        {
                            Currency ="usd",
                            Value = 50
                        },
                        Weight = new WeightModel
                        {
                            Unit ="g",
                            Value = 5
                        }
                    }
                }
            };
        }

    }
}
