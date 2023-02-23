﻿using MicroStore.IdentityProvider.Identity.Application;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Infrastructure.Extensions;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework
{
    public class RoleRepository : IRoleRepository , ITransientDependency
    {
        private readonly ApplicationRoleManager _applicationRoleManager;

        public RoleRepository(ApplicationRoleManager applicationRoleManager)
        {
            _applicationRoleManager = applicationRoleManager;
        }

        public async Task<ApplicationIdentityRole> CreateAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default)
        {
            var result = await _applicationRoleManager.CreateAsync(role);

            result.ThorwIfInvalidResult();

            return role;
        }

        public async Task<ApplicationIdentityRole?> FindById(string roleId, CancellationToken cancellationToken = default)
        {
            var role = await _applicationRoleManager.FindByIdAsync(roleId);

            return role;
        }

        public async Task<ApplicationIdentityRole?> FindByName(string? name, CancellationToken cancellationToken = default)
        {
            var role = await _applicationRoleManager.FindByNameAsync(name);

            return role;
        }

        public async Task<ApplicationIdentityRole> UpdateAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default)
        {
            var result = await _applicationRoleManager.UpdateAsync(role);

            result.ThorwIfInvalidResult();

            return role;
        }
    }
}