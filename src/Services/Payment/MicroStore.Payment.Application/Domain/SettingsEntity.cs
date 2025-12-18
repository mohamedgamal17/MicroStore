using Volo.Abp.Domain.Entities;
namespace MicroStore.Payment.Application.Domain
{
    public class SettingsEntity : Entity<Guid>
    {
        public string ProviderKey { get; set; }
        public string Data { get; set; } = "{}";


        private SettingsEntity() { }

        public SettingsEntity(string providerKey, string data)
        {
            ProviderKey = providerKey;
            Data = data;
        }


    }
}
