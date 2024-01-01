using AutoMapper.QueryableExtensions;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public class ApiScopeQueryService : IdentityServiceApplicationService, IApiScopeQueryService
    {
        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ApiScopeQueryService(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }
        public async Task<Result<List<ApiScopeDto>>> ListAsync(ApiScopeListQueryModel queryParamss ,CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiScopes.AsNoTracking().ProjectTo<ApiScopeDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(queryParamss.Name))
            {
                query = query.Where(x => x.Name.Contains(queryParamss.Name));
            }

            var result = await query.ToListAsync( cancellationToken);

            return result;
        }
        public async Task<Result<ApiScopeDto>> GetAsync(int apiScopeId, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiScopes.AsNoTracking().ProjectTo<ApiScopeDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (result == null)
            {
                return new Result<ApiScopeDto>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            return result;
        }

        public async Task<Result<List<ApiScopePropertyDto>>> ListProperties(int apiScopeId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(apiScopeId);

            if (result.IsFailure)
            {
                return new Result<List<ApiScopePropertyDto>>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            return result.Value.Properties;
        }

        public async Task<Result<ApiScopePropertyDto>> GetProperty(int apiScopeId, int propertyId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(apiScopeId);

            if (result.IsFailure)
            {
                return new Result<ApiScopePropertyDto>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            var property = result.Value.Properties.SingleOrDefault(x => x.Id == propertyId);

            if(property == null)
                return new Result<ApiScopePropertyDto>(new EntityNotFoundException(typeof(ApiScopeProperty), propertyId));

            return property;
        }
    }
}
