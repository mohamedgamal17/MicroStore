using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.Settings
{
    public interface IApplicationSettingsService : IApplicationService
    {
        Task<ShippingSettings> UpdateAsync(UpdateShippingSettingsModel model, CancellationToken cancellationToken = default);

        Task<ShippingSettings> GetAsync(CancellationToken cancellationToken = default);
    }
}
