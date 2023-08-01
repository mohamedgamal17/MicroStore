using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public class RoleCommandService : IdentityApplicationService, IRoleCommandService
    {
        private readonly RoleManager<ApplicationIdentityRole> _roleManager;
        public RoleCommandService(RoleManager<ApplicationIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result<IdentityRoleDto>> CreateAsync(RoleModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateRoleModel(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<IdentityRoleDto>(validationResult.Exception);
            }

            var identityRole = new ApplicationIdentityRole();

            PrepareRoleEntity(model, identityRole);

            var identityResult =  await _roleManager.CreateAsync(identityRole);

            identityResult.ThorwIfInvalidResult();

            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole);
        }

     

        public async Task<Result<IdentityRoleDto>> UpdateAsync(string roleId, RoleModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateRoleModel(model, roleId, cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<IdentityRoleDto>(validationResult.Exception);
            }

            var identityRole = await _roleManager.FindByIdAsync(roleId);

            if (identityRole == null)
            {
                return new Result<IdentityRoleDto>(new EntityNotFoundException(typeof(ApplicationIdentityRole), roleId));

            }

            PrepareRoleEntity(model, identityRole);

            await _roleManager.UpdateAsync(identityRole);
           
            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole);
        }

        private void PrepareRoleEntity(RoleModel model, ApplicationIdentityRole identityRole)
        {
            identityRole.Name = model.Name;

            identityRole.Description = model.Description;
        }

        private async Task<Result<Unit>> ValidateRoleModel(RoleModel model , string? roleId = null , CancellationToken cancellationToken = default)
        {
            var normalizedName = _roleManager.NormalizeKey(model.Name);

            var query = _roleManager.Roles;

            if(!string.IsNullOrEmpty(roleId))
            {
                query = query.Where(x => x.Id != roleId);
            }

            var role = await query.SingleOrDefaultAsync(x => x.NormalizedName == normalizedName, cancellationToken);

            if(role != null)
            {
                return new Result<Unit>(new UserFriendlyException($"Role name '{role.Name}' is already taken"));
            }


            return Unit.Value;
        }

        public async Task<Result<Unit>> RemoveAsync(string roleId, CancellationToken cancellationToken = default)
        {
            var identityRole = await _roleManager.FindByIdAsync(roleId);

            if(identityRole == null)
            {
                return  new Result<Unit>(new EntityNotFoundException(typeof(ApplicationIdentityRole), roleId));
            }

            await _roleManager.DeleteAsync(identityRole);

            return Unit.Value;

        }
    }
}
