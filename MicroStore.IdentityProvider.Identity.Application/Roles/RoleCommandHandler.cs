using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using System.Data;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public class RoleCommandHandler : RequestHandler,
        ICommandHandler<CreateRoleCommand, IdentityRoleDto>,
        ICommandHandler<UpdateRoleCommand, IdentityRoleDto>
    {
        private readonly ApplicationRoleManager _roleManager;

        public RoleCommandHandler(ApplicationRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseResult<IdentityRoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var identityRole = new ApplicationIdentityRole();

            PrepareRoleEntity(request, identityRole);

            var identityResult = await _roleManager.CreateAsync(identityRole);

            if (!identityResult.Succeeded)
            {
                return Failure<IdentityRoleDto>(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.Created, ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole));
        }

        public async Task<ResponseResult<IdentityRoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var identityRole = await _roleManager.FindByIdAsync(request.RoleId);

            if (identityRole == null)
            {
                return Failure<IdentityRoleDto>(HttpStatusCode.NotFound, $"Role with id {request.RoleId} is not exist");

            }

            PrepareRoleEntity(request, identityRole);

            var identityResult = await _roleManager.UpdateAsync(identityRole);

            if (!identityResult.Succeeded)
            {
                return Failure<IdentityRoleDto>(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole));
        }


        private void PrepareRoleEntity(RoleCommand request, ApplicationIdentityRole identityRole)
        {
            identityRole.Name = request.Name;

            identityRole.Description = request.Description;

            identityRole.RoleClaims = request.Claims?.Select(x => new ApplicationIdentityRoleClaim { ClaimType = x.Type, ClaimValue = x.Value }).ToList() ?? new List<ApplicationIdentityRoleClaim>();
        }
    }
}
