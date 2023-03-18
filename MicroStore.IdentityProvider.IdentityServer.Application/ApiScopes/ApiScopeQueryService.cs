using AutoMapper.QueryableExtensions;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
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
        public async Task<Result<PagedResult<ApiScopeDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiScopes.AsNoTracking().ProjectTo<ApiScopeDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

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
    }
}
