using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using System.Net;

namespace MicroStore.Shipping.Application.Settings
{
    public class ApplicationSettingsService : ShippingApplicationService, IApplicationSettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public ApplicationSettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<ShippingSettings> GetAsync(CancellationToken cancellationToken = default)
        {
            return await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey) ?? new ShippingSettings();
        }

        public async Task<ShippingSettings> UpdateAsync(UpdateShippingSettingsModel model, CancellationToken cancellationToken = default)
        {
            var settings = await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey, cancellationToken) ?? new ShippingSettings();

            settings.DefaultShippingSystem = model.DefaultShippingSystem;

            settings.Location = PrepareAddressSettings(model.Location);

            await _settingsRepository.TryToUpdateSettrings(settings, cancellationToken);

            return settings;
        }

        private AddressSettings PrepareAddressSettings(AddressModel? addressModel)
        {
            if (addressModel == null)
            {
                return new AddressSettings();
            }

            return new AddressSettings
            {
                Name = addressModel.Name,
                Phone = addressModel.Phone,
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
