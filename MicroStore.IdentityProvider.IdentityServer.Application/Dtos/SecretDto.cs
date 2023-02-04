using Volo.Abp.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Dtos
{
    public class SecretDto<TKey> : EntityDto<TKey>
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? Expiration { get; set; }
        public DateTime Created { get; set; }
    }
}
