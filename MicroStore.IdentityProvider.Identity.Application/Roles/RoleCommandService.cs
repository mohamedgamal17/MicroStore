using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Models;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public class RoleCommandService : IdentityApplicationService, IRoleCommandService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleCommandService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ResultV2<IdentityRoleDto>> CreateAsync(RoleModel model, CancellationToken cancellationToken = default)
        {
            var identityRole = new ApplicationIdentityRole();

            PrepareRoleEntity(model, identityRole);

           await _roleRepository.CreateAsync(identityRole);


            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole);
        }

        public async Task<ResultV2<IdentityRoleDto>> UpdateAsync(string roleId, RoleModel model, CancellationToken cancellationToken = default)
        {
            var identityRole = await _roleRepository.FindById(roleId);

            if (identityRole == null)
            {
                return new ResultV2<IdentityRoleDto>(new EntityNotFoundException(typeof(ApplicationIdentityRole), roleId));

            }

            PrepareRoleEntity(model, identityRole);

             await _roleRepository.UpdateAsync(identityRole);

           
            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(identityRole);
        }

        private void PrepareRoleEntity(RoleModel model, ApplicationIdentityRole identityRole)
        {
            identityRole.Name = model.Name;

            identityRole.Description = model.Description;

            identityRole.RoleClaims = model.Claims?.Select(x => new ApplicationIdentityRoleClaim { ClaimType = x.Type, ClaimValue = x.Value }).ToList() ?? new List<ApplicationIdentityRoleClaim>();
        }
    }
}
