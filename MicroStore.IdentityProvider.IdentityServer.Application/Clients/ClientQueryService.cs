using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public class ClientQueryService : IdentityServiceApplicationService, IClientQueryService
    {
        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ClientQueryService(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }

        public async Task<UnitResult<ClientDto>> GetAsync(int clientId, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.Clients.AsNoTracking().ProjectTo<ClientDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == clientId, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet with id : {clientId} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<PagedResult<ClientDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.Clients.AsNoTracking().ProjectTo<ClientDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResult.Success(result);

        }
    }
}
