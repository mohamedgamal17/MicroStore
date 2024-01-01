using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public class RoleQueryService : IdentityApplicationService, IRoleQueryService
    {
        private readonly IApplicationIdentityDbContext _identityDbContext;

        private readonly IRoleRepository _roleRepository;

        public RoleQueryService(IApplicationIdentityDbContext identityDbContext, IRoleRepository roleRepository)
        {
            _identityDbContext = identityDbContext;
            _roleRepository = roleRepository;
        }

        public async Task<Result<IdentityRoleDto>> GetAsync(string roleId, CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.FindById(roleId);

            if (role == null)
            {
                return new Result<IdentityRoleDto>(new EntityNotFoundException(typeof(ApplicationIdentityRole), roleId));
            }

            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(role);
        }

        public async Task<Result<IdentityRoleDto>> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.FindByName(roleName);

            if (role == null)
            {
                return new Result<IdentityRoleDto>(new EntityNotFoundException($"Role with name : {roleName} is not exist"));

            }

            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(role);
        }

        public async Task<Result<List<IdentityRoleDto>>> ListAsync(RoleListQueryModel queryParams,CancellationToken cancellationToken = default)
        {
            var query = _identityDbContext.Roles
                     .AsNoTracking()
                     .ProjectTo<IdentityRoleDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                query = query.Where(x => x.Name.Contains(queryParams.Name));
            }

            var result = await query.ToListAsync(cancellationToken);

            return result;
        }
    }
}
