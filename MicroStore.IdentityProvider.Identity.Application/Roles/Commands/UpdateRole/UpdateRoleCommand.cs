using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Application.Roles.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : ICommand<IdentityRoleDto>
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    internal class UpdateRoleCommandHandler : CommandHandler<UpdateRoleCommand, IdentityRoleDto>
    {
        private readonly RoleManager<ApplicationIdentityRole> _roleManager;

        public UpdateRoleCommandHandler(RoleManager<ApplicationIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override async Task<ResponseResult<IdentityRoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if(role == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Role with id {request.RoleId} is not exist");
            }

            role.Name = request.Name;

            role.Description = request.Description;

            var identityResult =  await _roleManager.UpdateAsync(role);

            if (!identityResult.Succeeded)
            {
                return Failure(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityRole,IdentityRoleDto>(role));
        }
    }
}
