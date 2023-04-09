﻿using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Models;
using Volo.Abp.Domain.Entities;
namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public class UserCommandService : IdentityApplicationService, IUserCommandService
    {
        private readonly ApplicationRoleManager _roleManager;

        private readonly IIdentityUserRepository _identityUserRepository;

        public UserCommandService(ApplicationRoleManager roleManager, IIdentityUserRepository identityUserRepository)
        {
            _roleManager = roleManager;
            _identityUserRepository = identityUserRepository;
        }

        public async Task<Result<IdentityUserDto>> CreateUserAsync(UserModel model, CancellationToken cancellationToken = default)
        {
            var applicationUser = new ApplicationIdentityUser();

            await PrepareUserEntity(model, applicationUser, cancellationToken);

            applicationUser.UserName = model.Email;

            await _identityUserRepository.CreateAsync(applicationUser, model.Password);

            return ObjectMapper.Map<ApplicationIdentityUser, IdentityUserDto>(applicationUser);
        }

        public async Task<Result<IdentityUserDto>> UpdateUserAsync(string userId, UserModel model, CancellationToken cancellationToken = default)
        {
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

    }
}
