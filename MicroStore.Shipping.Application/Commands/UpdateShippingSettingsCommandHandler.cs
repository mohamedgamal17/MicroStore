using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
namespace MicroStore.Shipping.Application.Commands
{
    public class UpdateShippingSettingsCommandHandler : CommandHandler<UpdateShippingSettingsCommand>
    {

        private readonly ISettingsRepository _settingsRepository;

        public UpdateShippingSettingsCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public override async Task<ResponseResult> Handle(UpdateShippingSettingsCommand request, CancellationToken cancellationToken)
        {
            var settings =await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey, cancellationToken) ?? new ShippingSettings();

            settings.DefaultShippingSystem = request.DefaultShippingSystem;

            settings.Location = PrepareAddressSettings(request.Location);

            await _settingsRepository.TryToUpdateSettrings(settings, cancellationToken);

            return Success(HttpStatusCode.Accepted);
        }


        private AddressSettings PrepareAddressSettings(AddressModel? addressModel)
        {
            if(addressModel == null)
            {
                return new AddressSettings();
            }

            return new AddressSettings
            {
                Name = addressModel.Name,
                Phone =addressModel.Phone,
                CountryCode = addressModel.CountryCode,
                State = addressModel.State,
                City = addressModel.City,
                Zip = addressModel.Zip,
                PostalCode = addressModel.PostalCode,
                AddressLine1 = addressModel.AddressLine1,
                AddressLine2 = addressModel.AddressLine2,
            };
        }
    }
}
