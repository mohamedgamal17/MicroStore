using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using Volo.Abp.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public class GetRoleWithIdQuery : IQuery<IdentityRoleDto>
    {
        public string Id { get; set; }
    }

    public class GetRoleWithNameQuery : IQuery<IdentityRoleDto>
    {
        public string Name { get; set; }
    }
    public class GetRoleListQuery : IQuery<ListResultDto<IdentityRoleDto>>
    {

    }
}
