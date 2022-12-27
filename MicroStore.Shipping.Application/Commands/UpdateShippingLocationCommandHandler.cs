using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using System.Net;

namespace MicroStore.Shipping.Application.Commands
{
    public class UpdateShippingLocationCommandHandler : CommandHandler<UpdateShippingLocationCommand>
    {

        private readonly ISettingsRepository _settingsRepository;

        public UpdateShippingLocationCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public override async Task<ResponseResult> Handle(UpdateShippingLocationCommand request, CancellationToken cancellationToken)
        {
            var settings = await  _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey, cancellationToken);

            if(settings == null)
            {
                settings = new ShippingSettings();
            }

            settings.Location.Name = request.Name;
            settings.Location.CountryCode = request.CountryCode;
            settings.Location.State = request.State;
            settings.Location.City = request.City;
            settings.Location.Zip= request.Zip;
            settings.Location.PostalCode = request.PostalCode;
            settings.Location.AddressLine1 = request.AddressLine1;
            settings.Location.AddressLine2 = request.AddressLine2;

            await _settingsRepository.TryToUpdateSettrings(settings,cancellationToken);

            return ResponseResult.Success((int) HttpStatusCode.Accepted) ;

        }
    }
}
