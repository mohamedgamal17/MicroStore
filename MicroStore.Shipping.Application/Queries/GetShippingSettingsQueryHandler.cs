using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Queries;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using System.Net;

namespace MicroStore.Shipping.Application.Queries
{
    public class GetShippingSettingsQueryHandler : QueryHandler<GetShippingSettingsQuery,ShippingSettings>
    {
        private readonly ISettingsRepository _settingsRepository;

        public GetShippingSettingsQueryHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public override async Task<ResponseResult<ShippingSettings>> Handle(GetShippingSettingsQuery request, CancellationToken cancellationToken)
        {
            var settings = await _settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey) ?? new ShippingSettings();

            return Success( HttpStatusCode.OK,settings);
        }
    }
}
