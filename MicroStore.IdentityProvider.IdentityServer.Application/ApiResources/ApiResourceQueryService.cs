using AutoMapper.QueryableExtensions;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Domain.Entities;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public class ApiResourceQueryService : IdentityServiceApplicationService, IApiResourceQueryService
    {
        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ApiResourceQueryService(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }

        public async Task<Result<PagedResult<ApiResourceDto>>> ListAsync(ApiResourceListQueryModel queryParams, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiResources
                .AsNoTracking()
                .ProjectTo<ApiResourceDto>(MapperAccessor.Mapper.ConfigurationProvider);


            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                query = query.Where(x => x.Name.Contains(queryParams.Name));
            }

            var result = await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

            return result;
        }

        public async Task<Result<ApiResourceDto>> GetAsync(int apiResourceId, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiResources.AsNoTracking().ProjectTo<ApiResourceDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (result == null)
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }
            return result;
        }

        public async Task<Result<List<ApiResourceSecretDto>>> ListApiResourceSecrets(int apiResourceId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(apiResourceId, cancellationToken);

            if (result.IsFailure)
            {
                return new Result<List<ApiResourceSecretDto>>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            return result.Value.Secrets;
        }

        public async Task<Result<ApiResourceSecretDto>> GetApiResourceSecret(int apiResourceId, int secretId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(apiResourceId,cancellationToken);

            if (result.IsFailure)
            {
                return new Result<ApiResourceSecretDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            var secret = result.Value.Secrets.SingleOrDefault(x => x.Id == secretId);

            if (secret == null)
            {
                return new Result<ApiResourceSecretDto>(new EntityNotFoundException(typeof(ApiResourceScopeDto), apiResourceId));
            }

            return secret;
        }

        public async Task<Result<List<ApiResourcePropertyDto>>> ListProperties(int apiResourceId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(apiResourceId);

            if (result.IsFailure)
            {
                return new Result<List<ApiResourcePropertyDto>>(new EntityNotFoundException(typeof(ApiScope), apiResourceId));
            }

            return result.Value.Properties;
        }

        public async Task<Result<ApiResourcePropertyDto>> GetApiResourceProperty(int apiResourceId, int propertyId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(apiResourceId);

            if (result.IsFailure)
            {
                return new Result<ApiResourcePropertyDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            var property = result.Value.Properties.SingleOrDefault(x => x.Id == propertyId);

            if (property == null)
                return new Result<ApiResourcePropertyDto>(new EntityNotFoundException(typeof(ApiScopeProperty), propertyId));

            return property;
        }
    }
}
