using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Application.Users.Dtos;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Users.Commands.AssingUserRoles
{
    public class AssingUserRolesCommand : ICommand<IdentityUserDto>
    {
        public Guid UserId { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }


    public class AssingUserRolesCommandHandler : CommandHandler<AssingUserRolesCommand, IdentityUserDto>
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        private readonly RoleManager<ApplicationIdentityRole> _roleManager;

        public AssingUserRolesCommandHandler(UserManager<ApplicationIdentityUser> userManager, RoleManager<ApplicationIdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override async Task<ResponseResult<IdentityUserDto>> Handle(AssingUserRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                return Failure(HttpStatusCode.NotFound, $"User with id : {request.UserId} is not exist");
            }

            var normalizedRoles = request.Roles.Select(x => _roleManager.NormalizeKey(x)).ToList();

            var roles = _roleManager.Roles.Where(x => normalizedRoles.Contains(x.NormalizedName));

            var result = user.AddUserRoles(roles);

            if (result.IsFailure)
            {
                return Failure(HttpStatusCode.BadRequest,result.Error);
            }


            var identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                return Failure(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.Created, ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(user));
        }

     
    }
}
