using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Commands
{
    public class AddApiResourceSecretCommand : SecretModel ,ICommand<ApiResourceDto>
    {
        public int ApiResourceId { get; set; }
    }


    public class RemoveApiResourceSecretCommand : ICommand<ApiResourceDto>
    {
        public int ApiResourceId { get; set;}
        public int SecretId { get; set; }
    }
}
