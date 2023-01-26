using Volo.Abp.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Common.Dtos
{
    public class IdentityClaimDto : EntityDto<int>
    {
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
    }
}
