using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public class UserCommandHandler : RequestHandler,
        ICommandHandler<CreateUserCommand, IdentityUserDto>,
        ICommandHandler<UpdateUserCommand, IdentityUserDto>
    {
        private readonly ApplicationRoleManager _roleManager;
        private readonly ApplicationUserManager _applicationUserManager;
        public UserCommandHandler(ApplicationRoleManager roleManager, ApplicationUserManager applicationUserManager)
        {
            _roleManager = roleManager;
            _applicationUserManager = applicationUserManager;
        }


        public async Task<ResponseResult<IdentityUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = new ApplicationIdentityUser();

            await PrepareUserEntity(request, applicationUser, cancellationToken);

            applicationUser.UserName = request.Email;

            var identityResult = await _applicationUserManager.CreateAsync(applicationUser);


            if (!identityResult.Succeeded)
            {
                return Failure<IdentityUserDto>(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            var passwordResult = await TryToAddPassword(request, applicationUser);

            if (!passwordResult.Succeeded)
            {
                return Failure<IdentityUserDto>(HttpStatusCode.BadRequest, passwordResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.Created, ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(applicationUser));
        }


        public async Task<ResponseResult<IdentityUserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _applicationUserManager.FindByIdAsync(request.UserId);

            if (applicationUser == null)
            {
                return Failure<IdentityUserDto>(HttpStatusCode.NotFound, $"User with id : {request.UserId} is not exist");
            }

            await PrepareUserEntity(request, applicationUser, cancellationToken);

            var identityResult = await _applicationUserManager.UpdateAsync(applicationUser);

            if (!identityResult.Succeeded)
            {
                return Failure<IdentityUserDto>(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }


            var passwordResult = await TryToAddPassword(request, applicationUser);

            if (!passwordResult.Succeeded)
            {
                return Failure<IdentityUserDto>(HttpStatusCode.BadRequest, identityResult.SerializeIdentityResultErrors());
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(applicationUser));


        }


        private async Task<ApplicationIdentityUser> PrepareUserEntity(UserCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = new ApplicationIdentityUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserClaims = request.UserClaims.Select(x => new ApplicationIdentityUserClaim { ClaimType = x.Type, ClaimValue = x.Value }).ToList()
            };

            var normalizedRoles = request.UserRoles.Select(x => _roleManager.NormalizeKey(x)).ToList();

            var roles = await _roleManager.Roles.Where(x => normalizedRoles.Contains(x.NormalizedName)).ToListAsync(cancellationToken);

            applicationUser.UserRoles = roles.Select(x => new ApplicationIdentityUserRole { RoleId = x.Id }).ToList();

            return applicationUser;
        }

        private async Task PrepareUserEntity(UserCommand request, ApplicationIdentityUser identityUser, CancellationToken cancellationToken)
        {
            identityUser.FirstName = request.FirstName;
            identityUser.LastName = request.LastName;
            identityUser.Email = request.Email;
            identityUser.PhoneNumber = request.PhoneNumber;
            identityUser.UserClaims = request.UserClaims?.Select(x => new ApplicationIdentityUserClaim { ClaimType = x.Type, ClaimValue = x.Value }).ToList() ?? new List<ApplicationIdentityUserClaim>();

            if (request.UserRoles != null)
            {
                var normalizedRoles = request.UserRoles.Select(x => _roleManager.NormalizeKey(x)).ToList();

                var roles = await _roleManager.Roles.Where(x => normalizedRoles.Contains(x.NormalizedName)).ToListAsync(cancellationToken);

                identityUser.UserRoles = roles.Select(x => new ApplicationIdentityUserRole { RoleId = x.Id }).ToList();
            }

        }

        private async Task RemoveUserRelations(ApplicationIdentityUser user)
        {
            var userClaims = await _applicationUserManager.GetClaimsAsync(user);

            var userRoles = await _applicationUserManager.GetRolesAsync(user);

            await _applicationUserManager.RemoveClaimsAsync(user, userClaims);

            await _applicationUserManager.RemoveFromRolesAsync(user, userRoles);

        }


        private async Task<IdentityResult> TryToAddPassword(UserCommand request, ApplicationIdentityUser identityUser)
        {
            if (request.Password != null)
            {

                return await _applicationUserManager.UpdateUserPasswordAsync(identityUser, request.Password);
            }


            return IdentityResult.Success;
        }
    }
}
