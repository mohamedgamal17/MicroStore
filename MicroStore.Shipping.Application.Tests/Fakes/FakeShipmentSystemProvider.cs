using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
namespace MicroStore.Shipping.Application.Tests.Fakes
{
    public class FakeShipmentSystemProvider : IShipmentSystemProvider
    {
        public string SystemName => FakeConst.ActiveSystem;

        private readonly IShipmentRepository _shipmentRepsoitory;

        private readonly IObjectMapper _objectMapper;

        public FakeShipmentSystemProvider(IShipmentRepository shipmentRepsoitory, IObjectMapper objectMapper)
        {
            _shipmentRepsoitory = shipmentRepsoitory;
            _objectMapper = objectMapper;
        }

        public async Task<ResponseResult> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model , CancellationToken cancellationToken = default)
        {
            Shipment shipment = await _shipmentRepsoitory.SingleOrDefaultAsync(x=> x.ShipmentExternalId == externalShipmentId);

            shipment.BuyShipmentLabel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            await _shipmentRepsoitory.UpdateAsync(shipment);

            return Success(_objectMapper.Map<Shipment, ShipmentDto>(shipment));
        }

        public Task<ResponseResult> EstimateShipmentRate(EstimatedRateModel model, CancellationToken cancellationToken = default)
        {
            var result = new List<EstimatedRateDto>
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
            };

            return Task.FromResult(Success(result));
        }

        public async Task<ResponseResult> Fullfill(Guid shipmentId, FullfillModel model , CancellationToken cancellationToken =default)
        {
            Shipment shipment = await _shipmentRepsoitory.RetriveShipment(shipmentId);

            shipment.Fullfill(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            await _shipmentRepsoitory.UpdateAsync(shipment);

            return  Success(PrepareShipmentFullfilledDto(model.AddressFrom, shipment));
        }

        public Task<bool> IsActive(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<ResponseResult> RetriveShipmentRates(string externalShipmentId,CancellationToken cancellationToken= default)
        {
            var result = new List<ShipmentRateDto>
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
            };

            return Task.FromResult(Success(result));
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

     

        public Task<ResponseResult> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default)
        {
            return Task.FromResult(ResponseResult.Success((int)HttpStatusCode.OK));
        }


        private ResponseResult Success<T>(T result)
        {
            return ResponseResult.Success((int)HttpStatusCode.OK, result);
        }
    }

    public class FakeNotActiveShipmentSystem : IShipmentSystemProvider 
    {
        public string SystemName => FakeConst.NotActiveSystem;

        public Task<ResponseResult> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult> EstimateShipmentRate(EstimatedRateModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult> Fullfill(Guid shipmentId, FullfillModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult> RetriveShipmentRates(string externalShipmentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
