#pragma warning disable 8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Dtos
{
    public class ResourceDto<TKey, TClaim, TProperty> : EntityDto<TKey>
        where TClaim : class
        where TProperty : class
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<TClaim> UserClaims { get; set; } = new List<TClaim>();
        public List<TProperty> Properties { get; set; } = new List<TProperty>();
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }
    }
}
