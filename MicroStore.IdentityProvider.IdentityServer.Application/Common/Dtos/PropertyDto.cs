#pragma warning disable 8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Common.Dtos
{
    public class PropertyDto<TKey> : EntityDto<TKey>
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
