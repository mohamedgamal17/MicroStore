using AutoMapper.QueryableExtensions;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
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

        public async Task<ResultV2<PagedResult<ApiResourceDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiResources.AsNoTracking().ProjectTo<ApiResourceDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return result;
        }

        public async Task<ResultV2<ApiResourceDto>> GetAsync(int apiResourceId, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiResources.AsNoTracking().ProjectTo<ApiResourceDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (result == null)
            {
                return new ResultV2<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }
            return result;
        }  
    }
}
