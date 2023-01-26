using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Application.Users.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Users.Commands.RemoveUserRoles
{
    public class RemoveUserRolesCommand  : ICommand<IdentityUserDto>
    {
        public Guid UserId { get; set; }

        public List<string> Roles { get; set; }
    }


    internal class RemoveUserRolesCommandHandler : CommandHandler<RemoveUserRolesCommand, IdentityUserDto>
    {
        private readonly UserManager<ApplicationIdentityUser> _applicationUserManager;

        private readonly RoleManager<ApplicationIdentityRole> _roleManager;

        public RemoveUserRolesCommandHandler(UserManager<ApplicationIdentityUser> applicationUser, RoleManager<ApplicationIdentityRole> roleManager)
        {
            _applicationUserManager = applicationUser;
            _roleManager = roleManager;
        }

        public override async Task<ResponseResult<IdentityUserDto>> Handle(RemoveUserRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await _applicationUserManager.FindByIdAsync(request.UserId.ToString());

            if(user == null)
            {
                return Failure(HttpStatusCode.NotFound, $"User with id : {request.UserId} is not exist");
            }

            if (user == null)
            {
                return Failure(HttpStatusCode.NotFound, $"User with id : {request.UserId} is not exist");
            }

            var normalizedRoles = request.Roles.Select(x => _roleManager.NormalizeKey(x)).ToList();

            var roles = _roleManager.Roles.Where(x => normalizedRoles.Contains(x.NormalizedName));

            var result =  user.RemoveUserRoles(roles);

            if (result.IsFailure)
            {
                return Failure(HttpStatusCode.BadRequest, result.Error);
            }

            var identityResult = await _applicationUserManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                return Failure(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.OK,
                ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(await _applicationUserManager.FindByIdAsync(request.UserId.ToString())));
        }
    }
}
