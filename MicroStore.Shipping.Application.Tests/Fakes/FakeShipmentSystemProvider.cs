using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Shipping.Application.Tests.Fakes
{
    public class FakeShipmentSystemProvider : IShipmentSystemProvider
    {
        public string SystemName => FakeConst.SystemName;

        private readonly IShipmentRepository _shipmentRepsoitory;

        private readonly IObjectMapper _objectMapper;

        public FakeShipmentSystemProvider(IShipmentRepository shipmentRepsoitory, IObjectMapper objectMapper)
        {
            _shipmentRepsoitory = shipmentRepsoitory;
            _objectMapper = objectMapper;
        }

        public async Task<ShipmentDto> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model , CancellationToken cancellationToken = default)
        {
            Shipment shipment = await _shipmentRepsoitory.SingleOrDefaultAsync(x=> x.ShipmentExternalId == externalShipmentId);

            shipment.BuyShipmentLabel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            await _shipmentRepsoitory.UpdateAsync(shipment);

            return _objectMapper.Map<Shipment, ShipmentDto>(shipment);
        }

        public Task<List<EstimatedRateDto>> EstimateShipmentRate(EstimatedRateModel model)
        {
            return Task.FromResult(new List<EstimatedRateDto>
            {
                new EstimatedRateDto
                {
                    EstaimatedDays = 3,
                    Name = Guid.NewGuid().ToString(),
                    Money = new MoneyDto
                    {
                        Value = 50,
                        Currency = "usd"
                    }
                },

                new EstimatedRateDto
                {
                    EstaimatedDays = 20,
                    Name = Guid.NewGuid().ToString(),
                    Money = new MoneyDto
                    {
                        Value = 30,
                        Currency = "usd"
                    }
                },
            });
        }

        public async Task<ShipmentFullfilledDto> Fullfill(Guid shipmentId, FullfillModel model , CancellationToken cancellationToken =default)
        {
            Shipment shipment = await _shipmentRepsoitory.RetriveShipment(shipmentId);

            shipment.Fullfill(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            await _shipmentRepsoitory.UpdateAsync(shipment);

            return PrepareShipmentFullfilledDto(model.AddressFrom, shipment);
        }

        public Task<bool> IsActive(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<List<ShipmentRateDto>> RetriveShipmentRates(string externalShipmentId)
        {
            return Task.FromResult(new List<ShipmentRateDto>
            {
                new ShipmentRateDto
                {
                    RateId = Guid.NewGuid().ToString(),
                    ServiceLevel = new ServiceLevelDto
                    {
                        Code ="NAN",
                        Name = "NAN",
                    },
                    Amount = new MoneyDto
                    {
                        Value = 50,
                        Currency = "usd",
                    },

                    CarrierId = Guid.NewGuid().ToString(),
                    Days = 5,
                }
            });
        }

        private ShipmentFullfilledDto PrepareShipmentFullfilledDto(AddressModel addresFrom ,Shipment shipment)
        {
            return new ShipmentFullfilledDto
            {
                ShipmentId = shipment.Id,
                ExternalShipmentId = shipment.ShipmentExternalId,
                AddressFrom = new AddressDto
                {
                    Name = addresFrom.Name,
                    Phone = addresFrom.Phone,
                    CountryCode = addresFrom.CountryCode,
                    City = addresFrom.City,
                    State = addresFrom.State,
                    PostalCode = addresFrom.PostalCode,
                    Zip = addresFrom.Zip,
                    AddressLine1 = addresFrom.AddressLine1,
                    AddressLine2 = addresFrom.AddressLine2

                },

                AddressTo = new AddressDto
                {
                    Name = shipment.Address.Name,
                    Phone = shipment.Address.Phone,
                    CountryCode = shipment.Address.CountryCode,
                    City = shipment.Address.City,
                    State = shipment.Address.State,
                    PostalCode = shipment.Address.PostalCode,
                    Zip = shipment.Address.Zip,
                    AddressLine1 = shipment.Address.AddressLine1,
                    AddressLine2 = shipment.Address.AddressLine2
                },
                Items = new List<ShipmentItemDto>()
            };
        }

        public Task<List<CarrierModel>> ListCarriers(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeNotActiveShipmentSystem : IShipmentSystemProvider 
    {
        public string SystemName => FakeConst.NotActiveSystem;

        public Task<ShipmentDto> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<EstimatedRateDto>> EstimateShipmentRate(EstimatedRateModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ShipmentFullfilledDto> Fullfill(Guid shipmentId, FullfillModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsActive(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }

        public Task<List<CarrierModel>> ListCarriers(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShipmentRateDto>> RetriveShipmentRates(string externalShipmentId)
        {
            throw new NotImplementedException();
        }
    }
}
