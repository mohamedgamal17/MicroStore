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

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public class ApiResourceQueryService : IdentityServiceApplicationService, IApiResourceQueryService
    {
        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ApiResourceQueryService(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }

        public async Task<Result<PagedResult<ApiResourceDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.ApiResources.AsNoTracking().ProjectTo<ApiResourceDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.Skip, queryParams.Lenght, cancellationToken);

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
    }
}
