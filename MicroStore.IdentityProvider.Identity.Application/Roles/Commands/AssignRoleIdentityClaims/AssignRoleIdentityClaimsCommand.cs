using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common.Models;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Roles.Dtos;
using System.Net;
using System.Security.Claims;

namespace MicroStore.IdentityProvider.Identity.Application.Roles.Commands.AssignRoleIdentityClaims
{
    public class AssignRoleIdentityClaimsCommand : ICommand<IdentityRoleDto>
    {
        public Guid RoleId { get; set; }

        public List<IdentityClaimModel> Claims { get; set; }
    }

    internal class AssignRoleIdentityClaimsCommandHandler : CommandHandler<AssignRoleIdentityClaimsCommand, IdentityRoleDto>
    {
        private readonly RoleManager<ApplicationIdentityRole> _roleManager;

        public AssignRoleIdentityClaimsCommandHandler(RoleManager<ApplicationIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override async Task<ResponseResult<IdentityRoleDto>> Handle(AssignRoleIdentityClaimsCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            var claims = request.Claims.Select(x => new Claim(x.Type, x.Value)).ToList();

            if(role == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Role with id : {request.RoleId} is not exist");
            }

            role.AddClaims(claims);

            await _roleManager.UpdateAsync(role);      
            

            return Success(HttpStatusCode.OK,ObjectMapper.Map<ApplicationIdentityRole,IdentityRoleDto>(role));
        }
    }
}
