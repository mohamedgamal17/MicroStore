using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Shipping.Application.Shipments
{
    public class ShipmentCommandService : ShippingApplicationService, IShipmentCommandService
    {
        private readonly IRepository<Shipment> _shipmentRepository;
        private readonly IShipmentSystemResolver _shipmentSystemResolver;
        private readonly ISettingsRepository _settingsRepository;

        public ShipmentCommandService(IRepository<Shipment> shipmentRepository, IShipmentSystemResolver shipmentSystemResolver, ISettingsRepository settingsRepository)
        {
            _shipmentRepository = shipmentRepository;
            _shipmentSystemResolver = shipmentSystemResolver;
            _settingsRepository = settingsRepository;
        }

        public async Task<UnitResult<ShipmentDto>> CreateAsync(ShipmentModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateShipment(model);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<ShipmentDto>(validationResult.Error);
            }
            Shipment shipment = new Shipment(model.OrderId,model.OrderNumber , model.UserId, model.Address.AsAddress())
            {
                Items = model.Items.Select(x => x.AsShipmentItem()).ToList(),
            };

            await _shipmentRepository.InsertAsync(shipment);


            return UnitResult.Success(ObjectMapper.Map<Shipment, ShipmentDto>(shipment));
        }
     
    
        public async Task<UnitResult<ShipmentDto>> FullfillAsync(string shipmentId, PackageModel model, CancellationToken cancellationToken = default)
        {
            var settings = await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey, cancellationToken) ?? new ShippingSettings();

            if (settings.DefaultShippingSystem == null )
            {
                return UnitResult.Failure<ShipmentDto>(ErrorInfo.BusinessLogic("Please configure default shipping system first"));
            }

            if(settings.Location == null)
            {
                return UnitResult.Failure<ShipmentDto>(ErrorInfo.BusinessLogic("Please configure location address first"));
            }


            var shipment = await _shipmentRepository.SingleOrDefaultAsync(x => x.Id == shipmentId, cancellationToken);

            if (shipment == null)
            {
                return UnitResult.Failure<ShipmentDto>(ErrorInfo.NotFound($"Shipment with id : {shipmentId} is not exist"));
            }

            if(shipment.Status != ShipmentStatus.Created)
            {
                return UnitResult.Failure<ShipmentDto>(ErrorInfo.BusinessLogic($"Shipment status should be in {ShipmentStatus.Created}"));
            }

            var systemResult = await _shipmentSystemResolver.Resolve(settings.DefaultShippingSystem, cancellationToken);

            if (systemResult.IsFailure)
            {
                return UnitResult.Failure<ShipmentDto>(systemResult.Error);
            }

            var system = systemResult.Value;

            var fullfillModel = PrepareFullfillModel(settings, shipment, model);

            return await system.Fullfill(shipmentId, fullfillModel);

        }
        public async Task<UnitResult<List<ShipmentRateDto>>> RetriveShipmentRatesAsync(string shipmentId, CancellationToken cancellationToken = default)
        {
            var shipment = await _shipmentRepository.SingleOrDefaultAsync(x => x.Id == shipmentId, cancellationToken);

            if (shipment == null)
            {
                return UnitResult.Failure<List<ShipmentRateDto>>(ErrorInfo.NotFound($"Shipment with id : {shipmentId} is not exist"));
            }

            if (shipment.Status != ShipmentStatus.Fullfilled)
            {
                return UnitResult.Failure<List<ShipmentRateDto>>(ErrorInfo.BusinessLogic($"Shipment status should be in {ShipmentStatus.Fullfilled}"));
            }

            var systemResult = await _shipmentSystemResolver.Resolve(shipment.SystemName);

            if (systemResult.IsFailure)
            {
                return UnitResult.Failure<List<ShipmentRateDto>>(systemResult.Error);
            }

            var system = systemResult.Value;

            return await system.RetriveShipmentRates(shipment.Id, cancellationToken);
        }

        public async Task<UnitResult<ShipmentDto>> BuyLabelAsync(string shipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default)
        {
            var shipment = await _shipmentRepository.SingleOrDefaultAsync(x => x.Id == shipmentId, cancellationToken);

            if (shipment == null)
            {
                return UnitResult.Failure<ShipmentDto>(ErrorInfo.NotFound($"Shipment with id : {shipmentId} is not exist"));
            }

            if (shipment.Status != ShipmentStatus.Fullfilled)
            {
                return UnitResult.Failure<ShipmentDto>(ErrorInfo.BusinessLogic($"Shipment status should be in {ShipmentStatus.Fullfilled}"));
            }

            var systemResult = await _shipmentSystemResolver.Resolve(shipment.SystemName);

            if (systemResult.IsFailure)
            {
                return UnitResult.Failure<ShipmentDto>(systemResult.Error);
            }

            var system = systemResult.Value;

            return await system.BuyShipmentLabel(shipment.Id , model, cancellationToken);
        }

        private async Task<UnitResult> ValidateShipment(ShipmentModel model)
        {
            var query = await _shipmentRepository.GetQueryableAsync();


            if(await query.AnyAsync(x=> x.OrderId == model.OrderId))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"Shipment is already created for Order with id : {model.OrderId}"));
            }

            if(await query.AnyAsync(x=> x.OrderNumber == model.OrderNumber))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"Shipment is already created for Order with numer : {model.OrderNumber}"));
            }

            return UnitResult.Success();
        }

        private FullfillModel PrepareFullfillModel(ShippingSettings settings, Shipment shipment ,  PackageModel model)
        {
            return new FullfillModel
            {
                AddressFrom = new AddressModel
                {
                    Name = settings.Location!.Name,
                    CountryCode = settings.Location!.CountryCode,
                    State = settings.Location!.State,
                    City = settings.Location!.City,
                    AddressLine1 = settings.Location!.AddressLine1,
                    AddressLine2 = settings.Location!.AddressLine2,
                    PostalCode = settings.Location!.PostalCode,
                    Zip = settings.Location!.Zip,
                    Phone = settings.Location!.Phone

                },

                AddressTo = new AddressModel
                {
                    Name = settings.Location!.Name,
                    CountryCode = settings.Location!.CountryCode,
                    State = settings.Location!.State,
                    City = settings.Location!.City,
                    AddressLine1 = settings.Location!.AddressLine1,
                    AddressLine2 = settings.Location!.AddressLine2,
                    PostalCode = settings.Location!.PostalCode,
                    Zip = settings.Location!.Zip,
                    Phone = settings.Location!.Phone
                },

                Package = model
            };
        }

      
    }
}
