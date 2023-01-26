using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Roles.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Roles.Queries.GetRole
{
    public class GetRoleWithNameQuery : IQuery<IdentityRoleDto>
    {
        public string Name { get; set; }
    }

    internal class GetRoleWithNameQueryHandler : QueryHandler<GetRoleWithNameQuery, IdentityRoleDto>
    {
        private readonly RoleManager<ApplicationIdentityRole> _roleManager;

        public GetRoleWithNameQueryHandler(RoleManager<ApplicationIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override async Task<ResponseResult<IdentityRoleDto>> Handle(GetRoleWithNameQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(request.Name);

            if (role == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Role with name : {request.Name} is not exist");
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(role));
        }
    }


}
