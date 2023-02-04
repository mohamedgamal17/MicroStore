using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public class ApiScopeQueryHandler : RequestHandler,
         IQueryHandler<GetApiScopeListQuery, PagedResult<ApiScopeDto>>,
         IQueryHandler<GetApiScopeQuery, ApiScopeDto>
    {
        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ApiScopeQueryHandler(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }

        public async Task<ResponseResult<PagedResult<ApiScopeDto>>> Handle(GetApiScopeListQuery request, CancellationToken cancellationToken)
        {
            var query = _applicationConfigurationDbContext.ApiScopes.AsNoTracking().ProjectTo<ApiScopeDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<ApiScopeDto>> Handle(GetApiScopeQuery request, CancellationToken cancellationToken)
        {
            var query = _applicationConfigurationDbContext.ApiScopes.AsNoTracking().ProjectTo<ApiScopeDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.ApiScopeId, cancellationToken);

            if (result == null)
            {
                return Failure<ApiScopeDto>(HttpStatusCode.NotFound, $"Api Scope with id : {request.ApiScopeId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
