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

namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public class ClientQueryService : IdentityServiceApplicationService, IClientQueryService
    {
        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ClientQueryService(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }

        public async Task<Result<ClientDto>> GetAsync(int clientId, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.Clients.AsNoTracking().ProjectTo<ClientDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == clientId, cancellationToken);

            if (result == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            return result;
        }

        public async Task<Result<PagedResult<ClientDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.Clients.AsNoTracking().ProjectTo<ClientDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return result;

        }
    }
}
