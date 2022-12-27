﻿using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
namespace MicroStore.Shipping.Application.Commands
{
    public class EstimateShipmentRateCommandHandler : CommandHandler<EstimateShipmentRateCommand>
    {
        private readonly IShipmentSystemResolver _shipmentSystemResolver;


        private readonly ISettingsRepository _settingsRepository;

        public EstimateShipmentRateCommandHandler(IShipmentSystemResolver shipmentSystemResolver, ISettingsRepository settingsRepository)
        {
            _shipmentSystemResolver = shipmentSystemResolver;
            _settingsRepository = settingsRepository;
        }

        public override async Task<ResponseResult> Handle(EstimateShipmentRateCommand request, CancellationToken cancellationToken)
        {
            var settings = await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey, cancellationToken);

            if (settings == null)
            {
                return ResponseResult.Failure((int)HttpStatusCode.BadRequest, new ErrorInfo
                {
                    Message = "Store Location is not provided"
                });
            }

            var model = new EstimatedRateModel
            {
                AddressFrom = PrepareAddressFrom(settings.Location),
                AddressTo = request.Address,
                Items = request.Items
            };

            var estimatedRates = await _shipmentSystemResolver.AggregateEstimationRate(model, cancellationToken);

            return ResponseResult.Success((int)HttpStatusCode.Accepted, estimatedRates);
        }

        private AddressModel PrepareAddressFrom(AddressSettings addressSettings)
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
    }
}
