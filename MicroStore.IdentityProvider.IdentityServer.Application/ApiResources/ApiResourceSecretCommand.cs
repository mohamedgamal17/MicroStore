using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public class AddApiResourceSecretCommand : SecretModel, ICommand<ApiResourceDto>
    {
        public int ApiResourceId { get; set; }
    }


    public class RemoveApiResourceSecretCommand : ICommand<ApiResourceDto>
    {
        public int ApiResourceId { get; set; }
        public int SecretId { get; set; }
    }
}
