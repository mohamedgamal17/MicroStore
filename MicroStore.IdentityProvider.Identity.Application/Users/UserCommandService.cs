using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public class UserCommandService : IdentityApplicationService, IUserCommandService
    {
        private readonly ApplicationRoleManager _roleManager;

        private readonly IIdentityUserRepository _identityUserRepository;

        private readonly UserManager<ApplicationIdentityUser> _userManager;
        public UserCommandService(ApplicationRoleManager roleManager, IIdentityUserRepository identityUserRepository, UserManager<ApplicationIdentityUser> userManager)
        {
            _roleManager = roleManager;
            _identityUserRepository = identityUserRepository;
            _userManager = userManager;
        }

        public async Task<Result<IdentityUserDto>> CreateUserAsync(UserModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateUserModel(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<IdentityUserDto>(validationResult.Exception);
            }

            var applicationUser = new ApplicationIdentityUser();

            await PrepareUserEntity(model, applicationUser, cancellationToken);

            applicationUser.UserName = model.Email;

            await _identityUserRepository.CreateAsync(applicationUser, model.Password);

            return ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(applicationUser);
        }


        public async Task<Result<IdentityUserDto>> UpdateUserAsync(string userId, UserModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateUserModel(model,userId ,cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<IdentityUserDto>(validationResult.Exception);
            }

            var applicationUser = await _identityUserRepository.FindById(userId, cancellationToken);

            if (applicationUser == null)
            {
                return new Result<IdentityUserDto>(new EntityNotFoundException(typeof(ApplicationIdentityUser), userId));
            }

            await PrepareUserEntity(model, applicationUser, cancellationToken);

            await _identityUserRepository.UpdateAsync(applicationUser, model.Password);

            return ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(applicationUser);
        }



        private async Task PrepareUserEntity(UserModel model, ApplicationIdentityUser identityUser, CancellationToken cancellationToken)
        {
            identityUser.GivenName = model.GivenName;

            identityUser.FamilyName = model.FamilyName;

            identityUser.Email = model.Email;

            identityUser.PhoneNumber = model.PhoneNumber;

            if (model.UserRoles != null)
            {
                var normalizedRoles = model.UserRoles.Select(x => _roleManager.NormalizeKey(x)).ToList();

                var roles = await _roleManager.Roles.Where(x => normalizedRoles.Contains(x.NormalizedName)).ToListAsync(cancellationToken);

                identityUser.UserRoles = roles.Select(x => new ApplicationIdentityUserRole { RoleId = x.Id }).ToList();
            }

        }

        private async Task<Result<Unit>> ValidateUserModel(UserModel model, string? userId =null, CancellationToken cancellationToken = default)
        {
            var normalizedUserName = _userManager.NormalizeName(model.Email);
            var normalizedEmail = _userManager.NormalizeEmail(model.Email);

            var query = _userManager.Users;

            if(userId != null)
            {
                query = query.Where(x => x.Id != userId);
            }

            if(await query.AnyAsync(x=> x.UserName == normalizedUserName))
            {
                return new Result<Unit>(new UserFriendlyException($"User name '{model.Email}' is already taken"));
            }

            if(await query.AnyAsync(x=> x.NormalizedEmail == normalizedEmail))
            {
                return new Result<Unit>(new UserFriendlyException($"email '{model.Email}' is already taken"));
            }

            return Unit.Value;
        }

    }
}
