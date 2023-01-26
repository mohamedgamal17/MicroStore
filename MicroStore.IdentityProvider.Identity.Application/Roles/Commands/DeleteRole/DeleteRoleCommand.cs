using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommand : ICommand
    {
        public string RoleId { get; set; }

    }

    public class DeleteRoleCommandHandler : CommandHandler<DeleteRoleCommand>
    {
        private readonly RoleManager<ApplicationIdentityUserRole> _roleManager;

        public DeleteRoleCommandHandler(RoleManager<ApplicationIdentityUserRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override async Task<ResponseResult<Unit>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId);

            if (role == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Role with id :{request.RoleId} is not exist");
            }

            var identityResult = await _roleManager.DeleteAsync(role);


            if (!identityResult.Succeeded)
            {
                return Failure(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.NoContent);
        }
    }
}
