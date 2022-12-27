using MicroStore.Payment.Domain.Common;
namespace MicroStore.Payment.Application.Abstractions.Common
{
    public interface ISettingsRepository
    {
        Task<T?> TryToGetSettings<T>(string providerKey, CancellationToken cancellationToken = default) where T : ISettings;

        Task<object?> TryToGetSettings(string providerKey, Type type, CancellationToken cancellationToken = default);

        Task TryToUpdateSettings<T>(T settings, CancellationToken cancellationToken = default) where T : ISettings;

        Task TryToUpdateSettrings(object settings, CancellationToken cancellationToken = default);
    }
}
