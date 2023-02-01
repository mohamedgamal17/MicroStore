using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using System.Net;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Queries
{
    public class ApiResourceQueryHandler : RequestHandler, 
        IQueryHandler<GetApiResourceListQuery, PagedResult<ApiResourceDto>>,
        IQueryHandler<GetApiResourceQuery, ApiResourceDto>
    {

        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ApiResourceQueryHandler(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }

        public async Task<ResponseResult<PagedResult<ApiResourceDto>>> Handle(GetApiResourceListQuery request, CancellationToken cancellationToken)
        {
            var query = _applicationConfigurationDbContext.ApiResources.AsNoTracking().ProjectTo<ApiResourceDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await  query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<ApiResourceDto>> Handle(GetApiResourceQuery request, CancellationToken cancellationToken)
        {
            var query = _applicationConfigurationDbContext.ApiResources.AsNoTracking().ProjectTo<ApiResourceDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.ApiResourceId,cancellationToken);

            if(result == null)
            {
                return Failure<ApiResourceDto>(HttpStatusCode.NotFound, $"Api resource with id : {request.ApiResourceId} , is not exist");
            }
            
            return Success(HttpStatusCode.OK, result);
        }
    }
}
