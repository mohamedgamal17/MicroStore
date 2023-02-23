using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Models;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public class RoleCommandService : IdentityApplicationService, IRoleCommandService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleCommandService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<UnitResultV2<IdentityRoleDto>> CreateAsync(RoleModel model, CancellationToken cancellationToken = default)
        {
            var identityRole = new ApplicationIdentityRole();

            PrepareRoleEntity(model, identityRole);

           await _roleRepository.CreateAsync(identityRole);


            return UnitResultV2.Success(ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole));
        }

        public async Task<UnitResultV2<IdentityRoleDto>> UpdateAsync(string roleId, RoleModel model, CancellationToken cancellationToken = default)
        {
            var identityRole = await _roleRepository.FindById(roleId);

            if (identityRole == null)
            {
                return UnitResultV2.Failure<IdentityRoleDto>(ErrorInfo.NotFound($"Role with id {roleId} is not exist"));

            }

            PrepareRoleEntity(model, identityRole);

             await _roleRepository.UpdateAsync(identityRole);

           
            return UnitResultV2.Success(ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole));
        }

        private void PrepareRoleEntity(RoleModel model, ApplicationIdentityRole identityRole)
        {
            identityRole.Name = model.Name;

            identityRole.Description = model.Description;

            identityRole.RoleClaims = model.Claims?.Select(x => new ApplicationIdentityRoleClaim { ClaimType = x.Type, ClaimValue = x.Value }).ToList() ?? new List<ApplicationIdentityRoleClaim>();
        }
    }
}
