using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common.Models;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Application.Roles.Dtos;
using System.Net;
using System.Security.Claims;
namespace MicroStore.IdentityProvider.Identity.Application.Roles.Commands.RemoveRoleIdentityClaims
{
    public class RemoveRoleIdentityClaimsCommand : ICommand<IdentityRoleDto>
    {
        public Guid RoleId { get; set; }
        public List<IdentityClaimModel> Claims { get; set; }
    }

    public class RemoveRoleIdentityClaimsCommandHandler : CommandHandler<RemoveRoleIdentityClaimsCommand, IdentityRoleDto>
    {
        private readonly RoleManager<ApplicationIdentityRole> _roleManager;

        public RemoveRoleIdentityClaimsCommandHandler(RoleManager<ApplicationIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override async Task<ResponseResult<IdentityRoleDto>> Handle(RemoveRoleIdentityClaimsCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            var claims = request.Claims.Select(x => new Claim(x.Type, x.Value)).ToList();

            if (role == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Role with id : {request.RoleId} is not exist");
            }

            role.RemoveClaims(claims);


            var identityResult = await _roleManager.UpdateAsync(role);

            if (!identityResult.Succeeded)
            {
                return Failure(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(role));
        }
    }
}
