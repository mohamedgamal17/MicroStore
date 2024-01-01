using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using Volo.Abp.Domain.Entities;
namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public class UserCommandService : IdentityApplicationService, IUserCommandService
    {
        private readonly RoleManager<ApplicationIdentityRole> _roleManager;

        private readonly ApplicationUserManager _userManager;
        public UserCommandService(RoleManager<ApplicationIdentityRole> roleManager, ApplicationUserManager userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Result<IdentityUserDto>> CreateUserAsync(UserModel model, CancellationToken cancellationToken = default)
        {


            var applicationUser = new ApplicationIdentityUser();

            await PrepareUserEntity(model, applicationUser, cancellationToken);

            applicationUser.UserName = model.Email;

            IdentityResult identityResult;

            if(model.Password != null)
            {
                identityResult = await _userManager.CreateAsync(applicationUser, model.Password);
            }
            else
            {
                identityResult = await _userManager.CreateAsync(applicationUser);
            }

            if (!identityResult.Succeeded)
            {
                return identityResult.ConvertToResult<IdentityUserDto>();
            }

            return ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(applicationUser);
        }


        public async Task<Result<IdentityUserDto>> UpdateUserAsync(string userId, UserModel model, CancellationToken cancellationToken = default)
        {


            var applicationUser = await _userManager.FindByIdAsync(userId);

            if (applicationUser == null)
            {
                return new Result<IdentityUserDto>(new EntityNotFoundException(typeof(ApplicationIdentityUser), userId));
            }

            await PrepareUserEntity(model, applicationUser, cancellationToken);


            var identityResult =  await _userManager.UpdateAsync(applicationUser, model.Password);

            if (!identityResult.Succeeded)
            {
                return identityResult.ConvertToResult<IdentityUserDto>();
            }

            return ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(applicationUser);
        }



        private async Task PrepareUserEntity(UserModel model, ApplicationIdentityUser identityUser, CancellationToken cancellationToken)
        {
            identityUser.GivenName = model.GivenName;

            identityUser.FamilyName = model.FamilyName;

            identityUser.UserName = model.UserName;

            identityUser.Email = model.Email;

            identityUser.PhoneNumber = model.PhoneNumber;

            if (model.UserRoles != null)
            {
                var normalizedRoles = model.UserRoles.Select(x => _roleManager.NormalizeKey(x)).ToList();

                var roles = await _roleManager.Roles.Where(x => normalizedRoles.Contains(x.NormalizedName)).ToListAsync(cancellationToken);

                identityUser.UserRoles = roles.Select(x => new ApplicationIdentityUserRole { RoleId = x.Id }).ToList();
            }

        }


    }
}
