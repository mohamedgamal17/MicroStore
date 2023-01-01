using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Extensions;
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

            var result = await RetriveShippingSettings(cancellationToken);

            if (result.IsFailure)
            {
                return result;
            }

            var settings = result.GetEnvelopeResult<ShippingSettings>().Result;

            var model = new EstimatedRateModel
            {
                AddressFrom = PrepareAddressFrom(settings.Location!),
                AddressTo = request.Address,
                Items = request.Items
            };

            var systemResult = await _shipmentSystemResolver.Resolve(settings.DefaultShippingSystem, cancellationToken);

            if (systemResult.IsFailure)
            {
                return systemResult.ConvertFaildUnitResult();
            }

            var system = systemResult.Value;

            var estimatedRates = await system.EstimateShipmentRate(model, cancellationToken);

            return estimatedRates;
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


        private async Task<ResponseResult> RetriveShippingSettings(CancellationToken cancellationToken)
        {
            var settings = await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey, cancellationToken) ?? new ShippingSettings();


            if (settings.DefaultShippingSystem == null)
            {
                return Failure(HttpStatusCode.BadRequest, new ErrorInfo
                {
                    Message = "Please set default shipping system"
                });
            }
            else if (settings.Location == null)
            {
                return Failure(HttpStatusCode.BadRequest, new ErrorInfo
                {
                    Message = "Please add store location"
                });
            }

            return Success(HttpStatusCode.OK, settings);

        }

    }
}
