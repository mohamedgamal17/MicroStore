#pragma warning disable 8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Common.Dtos
{
    public class UserClaimDto<TKey> : EntityDto<TKey>
    {
        public string Type { get; set; }

    }
}
