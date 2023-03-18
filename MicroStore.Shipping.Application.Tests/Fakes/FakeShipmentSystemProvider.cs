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

        public async Task<Result<ShipmentDto>> BuyShipmentLabel(string shippmentId, BuyShipmentLabelModel model , CancellationToken cancellationToken = default)
        {
            Shipment shipment = await _shipmentRepsoitory.SingleOrDefaultAsync(x=> x.Id == shippmentId);

            shipment.BuyShipmentLabel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            await _shipmentRepsoitory.UpdateAsync(shipment);

            return _objectMapper.Map<Shipment, ShipmentDto>(shipment);
        }

        public Task<Result<List<EstimatedRateDto>>> EstimateShipmentRate(AddressModel addressFrom , AddressModel addressTo , List<ShipmentItemEstimationModel> items, CancellationToken cancellationToken = default)
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

            return Task.FromResult(new Result<List<EstimatedRateDto>>(result));
        }

        public async Task<Result<ShipmentDto>> Fullfill(string shipmentId, FullfillModel model , CancellationToken cancellationToken =default)
        {
            Shipment shipment = await _shipmentRepsoitory.RetriveShipment(shipmentId);

            shipment.Fullfill(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            await _shipmentRepsoitory.UpdateAsync(shipment);

            return  _objectMapper.Map<Shipment, ShipmentDto>(shipment);
        }
   
        public Task<Result<List<ShipmentRateDto>>> RetriveShipmentRates(string shipmentId,CancellationToken cancellationToken= default)
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

            return Task.FromResult(new Result<List<ShipmentRateDto>>(result));
        }

 
        public Task<List<CarrierModel>> ListCarriers(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

     

        public Task<Result<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default)
        {
            return Task.FromResult(new Result<AddressValidationResultModel>( new AddressValidationResultModel()));
        }


     
    }

    public class FakeNotActiveShipmentSystem : IShipmentSystemProvider 
    {
        public string SystemName => FakeConst.NotActiveSystem;

        public Task<Result<ShipmentDto>> BuyShipmentLabel(string shipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<EstimatedRateDto>>> EstimateShipmentRate(AddressModel from, AddressModel to, List<ShipmentItemEstimationModel> items, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ShipmentDto>> Fullfill(string shipmentId, FullfillModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<ShipmentRateDto>>> RetriveShipmentRates(string shipmentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
