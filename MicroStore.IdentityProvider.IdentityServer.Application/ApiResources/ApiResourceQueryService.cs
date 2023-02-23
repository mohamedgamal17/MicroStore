using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public class ApiResourceQueryService : IdentityServiceApplicationService, IApiResourceQueryService
    {
        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ApiResourceQueryService(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }

        public async Task<UnitResult<PagedResult<ApiResourceDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiResources.AsNoTracking().ProjectTo<ApiResourceDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<ApiResourceDto>> GetAsync(int apiResourceId, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiResources.AsNoTracking().ProjectTo<ApiResourceDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<ApiResourceDto>(ErrorInfo.NotFound($"Api resource with id : {apiResourceId} , is not exist"));
            }
            return UnitResult.Success(result);
        }  
    }
}
