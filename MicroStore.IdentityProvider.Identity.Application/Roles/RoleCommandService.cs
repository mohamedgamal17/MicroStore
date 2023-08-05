using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Extensions;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
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
            var identityRole = new ApplicationIdentityRole();

            PrepareRoleEntity(model, identityRole);

            var identityResult =  await _roleManager.CreateAsync(identityRole);

            if (!identityResult.Succeeded)
            {
                return identityResult.ConvertToResult<IdentityRoleDto>();
            }

            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole);
        }

     

        public async Task<Result<IdentityRoleDto>> UpdateAsync(string roleId, RoleModel model, CancellationToken cancellationToken = default)
        {

            var identityRole = await _roleManager.FindByIdAsync(roleId);

            if (identityRole == null)
            {
                return new Result<IdentityRoleDto>(new EntityNotFoundException(typeof(ApplicationIdentityRole), roleId));

            }

            PrepareRoleEntity(model, identityRole);

            var identityResult =  await _roleManager.UpdateAsync(identityRole);

            if (!identityResult.Succeeded)
            {
                return identityResult.ConvertToResult<IdentityRoleDto>();
            }

            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole);
        }

        private void PrepareRoleEntity(RoleModel model, ApplicationIdentityRole identityRole)
        {
            identityRole.Name = model.Name;

            identityRole.Description = model.Description;
        }


        public async Task<Result<Unit>> RemoveAsync(string roleId, CancellationToken cancellationToken = default)
        {
            var identityRole = await _roleManager.FindByIdAsync(roleId);

            if(identityRole == null)
            {
                return  new Result<Unit>(new EntityNotFoundException(typeof(ApplicationIdentityRole), roleId));
            }

            var identityResult = await _roleManager.DeleteAsync(identityRole);

            if (!identityResult.Succeeded)
            {
                return identityResult.ConvertToResult<Unit>();
            }

            return Unit.Value;

        }
    }
}
