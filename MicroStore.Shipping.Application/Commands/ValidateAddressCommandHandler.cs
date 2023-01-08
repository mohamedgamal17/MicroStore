using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Extensions;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using System.Net;

namespace MicroStore.Shipping.Application.Commands
{
    public class ValidateAddressCommandHandler : CommandHandler<ValidateAddressCommand>
    {
        private readonly ISettingsRepository _settingsRepository;

        private readonly IShipmentSystemResolver _shipmentSystemResolver;

        public ValidateAddressCommandHandler(ISettingsRepository settingsRepository, IShipmentSystemResolver shipmentSystemResolver)
        {
            _settingsRepository = settingsRepository;
            _shipmentSystemResolver = shipmentSystemResolver;
        }

        public override async Task<ResponseResult> Handle(ValidateAddressCommand request, CancellationToken cancellationToken)
        {
            var settings = await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey, cancellationToken) ?? new ShippingSettings();

            if(settings.DefaultShippingSystem == null)
            {
                return Failure(HttpStatusCode.BadRequest, "Please configure shipping settings first");
            }

            var systemResult = await _shipmentSystemResolver.Resolve(settings.DefaultShippingSystem);

            if (systemResult.IsFailure)
            {
                return systemResult.ConvertFaildUnitResult();
            }

            var system = systemResult.Value;

            var model = PrepareAddressModel(request);

            return await system.ValidateAddress(model, cancellationToken);
        }


        private AddressModel PrepareAddressModel(ValidateAddressCommand command)
        {
            return new AddressModel
            {
                Name = command.Name,
                Phone = command.Phone,
                State = command.State,
                CountryCode = command.CountryCode,
                City = command.City,
                Zip = command.Zip,
                PostalCode = command.PostalCode,
                AddressLine1 = command.AddressLine1,
                AddressLine2 = command.AddressLine2,
            };
        }
    }
}
