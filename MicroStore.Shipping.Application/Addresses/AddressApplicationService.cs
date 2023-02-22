using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Application.Services;
namespace MicroStore.Shipping.Application.Addresses
{
    public class AddressApplicationService : ShippingApplicationService, IAddressApplicationService
    {
        private readonly ISettingsRepository _settingsRepository;

        private readonly IShipmentSystemResolver _shipmentSystemResolver;

        public AddressApplicationService(ISettingsRepository settingsRepository, IShipmentSystemResolver shipmentSystemResolver)
        {
            _settingsRepository = settingsRepository;
            _shipmentSystemResolver = shipmentSystemResolver;
        }

        public async Task<UnitResultV2<AddressValidationResultModel>> ValidateAddress(AddressModel model, CancellationToken cancellationToken = default)
        {
            var settings = await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey, cancellationToken) ?? new ShippingSettings();

            if (settings.DefaultShippingSystem == null)
            {
                return UnitResultV2.Failure<AddressValidationResultModel>(ErrorInfo.BusinessLogic("Please configure shipping settings first"));
            }

            var systemResult = await _shipmentSystemResolver.Resolve(settings.DefaultShippingSystem);

            if (systemResult.IsFailure)
            {
                return UnitResultV2.Failure<AddressValidationResultModel>(systemResult.Error);
            }

            var system = systemResult.Result;

            return await system.ValidateAddress(model, cancellationToken);
        }
    }


    public interface IAddressApplicationService : IApplicationService
    {
        Task<UnitResultV2<AddressValidationResultModel>> ValidateAddress(AddressModel model , CancellationToken cancellationToken  =default);
    }
}
