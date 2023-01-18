using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
using Volo.Abp.Application.Dtos;
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

        public async Task<ResponseResult<ShipmentDto>> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model , CancellationToken cancellationToken = default)
        {
            Shipment shipment = await _shipmentRepsoitory.SingleOrDefaultAsync(x=> x.ShipmentExternalId == externalShipmentId);

            shipment.BuyShipmentLabel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            await _shipmentRepsoitory.UpdateAsync(shipment);

            return Success(_objectMapper.Map<Shipment, ShipmentDto>(shipment));
        }

        public Task<ResponseResult<ListResultDto<EstimatedRateDto>>> EstimateShipmentRate(EstimatedRateModel model, CancellationToken cancellationToken = default)
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

            return Task.FromResult(Success(new ListResultDto<EstimatedRateDto>(result.AsReadOnly())));
        }

        public async Task<ResponseResult<ShipmentDto>> Fullfill(Guid shipmentId, FullfillModel model , CancellationToken cancellationToken =default)
        {
            Shipment shipment = await _shipmentRepsoitory.RetriveShipment(shipmentId);

            shipment.Fullfill(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            await _shipmentRepsoitory.UpdateAsync(shipment);

            return  Success(_objectMapper.Map<Shipment,ShipmentDto>(shipment));
        }
   
        public Task<ResponseResult<ListResultDto<ShipmentRateDto>>> RetriveShipmentRates(string externalShipmentId,CancellationToken cancellationToken= default)
        {
            var result = new List<ShipmentRateDto>
            {
                new ShipmentRateDto
                {
                    Id = Guid.NewGuid().ToString(),
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

            return Task.FromResult(Success(new ListResultDto<ShipmentRateDto>(result.AsReadOnly())));
        }

 
        public Task<List<CarrierModel>> ListCarriers(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

     

        public Task<ResponseResult<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default)
        {
            return Task.FromResult(ResponseResult.Success((int)HttpStatusCode.OK,new AddressValidationResultModel()));
        }


        private ResponseResult<T> Success<T>(T result)
        {
            return ResponseResult.Success((int)HttpStatusCode.OK, result);
        }
    }

    public class FakeNotActiveShipmentSystem : IShipmentSystemProvider 
    {
        public string SystemName => FakeConst.NotActiveSystem;

        public Task<ResponseResult<ShipmentDto>> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult<ListResultDto<EstimatedRateDto>>> EstimateShipmentRate(EstimatedRateModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult<ShipmentDto>> Fullfill(Guid shipmentId, FullfillModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult<ListResultDto<ShipmentRateDto>>> RetriveShipmentRates(string externalShipmentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
