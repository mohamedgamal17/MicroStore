﻿using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.Rates
{
    public class RateApplicationService :  ShippingApplicationService,IRateApplicationService
    {
        private readonly IShipmentSystemResolver _shipmentSystemResolver;

        private readonly ISettingsRepository _settingsRepository;

        public RateApplicationService(IShipmentSystemResolver shipmentSystemResolver, ISettingsRepository settingsRepository)
        {
            _shipmentSystemResolver = shipmentSystemResolver;
            _settingsRepository = settingsRepository;
        }

        public async Task<UnitResultV2<List<EstimatedRateDto>>> EstimateRate(EstimatedRateModel model, CancellationToken cancellationToken = default)
        {
            var result = await RetriveShippingSettings(cancellationToken);

            if (result.IsFailure)
            {
                return UnitResultV2.Failure<List<EstimatedRateDto>>(result.Error);
            }

            var settings = result.Result;

            var systemResult = await _shipmentSystemResolver.Resolve(settings.DefaultShippingSystem, cancellationToken);

            if (systemResult.IsFailure)
            {
                return UnitResultV2.Failure<List<EstimatedRateDto>>(result.Error);
            }


            var system = systemResult.Result;

            return await system.EstimateShipmentRate(PrepareLocation(settings.Location!), model.Address, model.Items);

        }

        private AddressModel PrepareLocation(AddressSettings addressSettings)
        {
            return new AddressModel
            {
                Name = addressSettings.Name,
                CountryCode = addressSettings.CountryCode,
                City = addressSettings.City,
                State = addressSettings.State,
                PostalCode = addressSettings.PostalCode,
                Zip = addressSettings.Zip,
                AddressLine1 = addressSettings.AddressLine1,
                AddressLine2 = addressSettings.AddressLine2,
            };
        }

        private async Task<UnitResultV2<ShippingSettings>> RetriveShippingSettings(CancellationToken cancellationToken)
        {
            var settings = await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey, cancellationToken) ?? new ShippingSettings();


            if (settings.DefaultShippingSystem == null)
            {
                return UnitResultV2.Failure<ShippingSettings>(ErrorInfo.BusinessLogic("Please set default shipping system"));
          
            }
            else if (settings.Location == null)
            {
                return UnitResultV2.Failure<ShippingSettings>(ErrorInfo.BusinessLogic("Please add store location"));
         
            }

            return UnitResultV2.Success(settings);

        }
    }



    public interface IRateApplicationService : IApplicationService
    {
        Task<UnitResultV2<List<EstimatedRateDto>>> EstimateRate(EstimatedRateModel model, CancellationToken cancellationToken = default);
    }
}