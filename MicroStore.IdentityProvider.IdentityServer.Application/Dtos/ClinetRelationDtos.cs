#pragma warning disable 8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Dtos
{
    public class ClientSecretDto : SecretDto<int>
    {
        public int ClientId { get; set; }
    }
    public class ClientGrantTypeDto : EntityDto<int>
    {
        public string GrantType { get; set; }
        public int ClientId { get; set; }
    }
    public class ClientRedirectUriDto : EntityDto<int>
    {
        public string RedirectUri { get; set; }

        public int ClientId { get; set; }
    }
    public class ClientPostLogoutRedirectUriDto : EntityDto<int>
    {
        public string PostLogoutRedirectUri { get; set; }
        public int ClientId { get; set; }
    }
    public class ScopeDto<TKey> : EntityDto<TKey>
    {
        public string Scope { get; set; }

    }
    public class ClientScopeDto : ScopeDto<int>
    {
        public int ClientId { get; set; }
    }
    public class ClientIdPRestrictionDto : EntityDto<int>
    {
        public string Provider { get; set; }
        public int ClientId { get; set; }
    }
    public class ClientCorsOriginDto
    {
        public string Origin { get; set; }
        public int ClientId { get; set; }
    }


    public class ClinetProperty : PropertyDto<int>
    {
        public int ClientId { get; set; }
    }

    public class ClientClaimDto : EntityDto<int>
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int ClientId { get; set; }
    }
}
