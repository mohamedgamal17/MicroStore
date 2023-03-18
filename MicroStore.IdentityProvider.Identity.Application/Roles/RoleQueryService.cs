using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
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

        public async Task<ResultV2<IdentityRoleDto>> GetAsync(string roleId, CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.FindById(roleId);

            if (role == null)
            {
                return new ResultV2<IdentityRoleDto>(new EntityNotFoundException(typeof(ApplicationIdentityRole), roleId));
            }

            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(role);
        }

        public async Task<ResultV2<IdentityRoleDto>> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.FindByName(roleName);

            if (role == null)
            {
                return new ResultV2<IdentityRoleDto>(new EntityNotFoundException($"Role with name : {roleName} is not exist"));

            }

            return ObjectMapper.Map<ApplicationIdentityRole, IdentityRoleDto>(role);
        }

        public async Task<ResultV2<List<IdentityRoleDto>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var query = _identityDbContext.Roles
                     .AsNoTracking()
                     .ProjectTo<IdentityRoleDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.ToListAsync(cancellationToken);

            return result;
        }
    }
}
