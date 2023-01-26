using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Roles.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Roles.Queries.GetRole
{
    public class GetRoleWithIdQuery :IQuery<IdentityRoleDto>
    {
        public Guid Id { get; set; }
    }


    public class GetRoleWithIdQueryHandler : QueryHandler<GetRoleWithIdQuery, IdentityRoleDto>
    {
        private readonly RoleManager<ApplicationIdentityRole> _roleManager;

        public GetRoleWithIdQueryHandler(RoleManager<ApplicationIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override async Task<ResponseResult<IdentityRoleDto>> Handle(GetRoleWithIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());

            if(role == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Role with id : {request.Id} is not exist");
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityRole,IdentityRoleDto>(role));
        }
    }
}
