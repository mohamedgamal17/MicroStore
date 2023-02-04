using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public abstract class ApiScopeCommand
    {
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public bool Emphasize { get; set; }
        public List<string>? UserClaims { get; set; }
        public List<PropertyModel>? Properties { get; set; }


    }
    public class CreateApiScopeCommand : ApiScopeCommand, ICommand<ApiScopeDto>
    {
    }
    public class UpdateApiScopeCommand : ApiScopeCommand, ICommand<ApiScopeDto>
    {
        public int ApiScopeId { get; set; }

    }


    public class RemoveApiScopeCommand : ICommand
    {
        public int ApiScopeId { get; set; }
    }

}
