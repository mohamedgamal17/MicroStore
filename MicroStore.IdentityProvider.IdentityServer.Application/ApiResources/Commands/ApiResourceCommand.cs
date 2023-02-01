using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Commands
{
    public abstract class ApiResourceCommand
    {
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public bool RequireResourceIndicator { get; set; }
        public List<PropertyModel>? Properties { get; set; }
        public List<string>? UserClaims { get; set; }
        public List<string>? Scopes { get; set; }


     
    }

    public class CreateApiResourceCommand  : ApiResourceCommand,  ICommand<ApiResourceDto>
    {
    }

    public class UpdateApiResourceCommand : ApiResourceCommand, ICommand<ApiResourceDto>
    {
        public int ApiResourceId { get; set; }

    }


    public class RemoveApiResourceCommand : ICommand
    {
        public int ApiResourceId { get; set; }

    }



}
