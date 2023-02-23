using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public class ApiScopeQueryService : IdentityServiceApplicationService, IApiScopeQueryService
    {
        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ApiScopeQueryService(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }
        public async Task<UnitResultV2<PagedResult<ApiScopeDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiScopes.AsNoTracking().ProjectTo<ApiScopeDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResultV2.Success(result);
        }
        public async Task<UnitResultV2<ApiScopeDto>> GetAsync(int apiScopeId, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiScopes.AsNoTracking().ProjectTo<ApiScopeDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (result == null)
            {
                return UnitResultV2.Failure<ApiScopeDto>(ErrorInfo.NotFound($"Api Scope with id : {apiScopeId} is not exist"));
            }

            return UnitResultV2.Success(result);
        }
    }
}
