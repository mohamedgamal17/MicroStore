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

  

        public async Task<Result<ClientSecretDto>> GetClientSecret(int clientId, int secretId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(clientId);

            if (result.IsFailure)
            {
                return new Result<ClientSecretDto>(result.Exception);
            }

            var secret = result.Value.ClientSecrets.SingleOrDefault(x => x.Id == clientId);

            if(secret == null)
            {
                return new Result<ClientSecretDto>(new EntityNotFoundException(typeof(ClientSecret), secretId));
            }

            return secret;
        }

     
        public async Task<Result<PagedResult<ClientDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _applicationConfigurationDbContext.Clients.AsNoTracking().ProjectTo<ClientDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(queryParams.Skip, queryParams.Lenght, cancellationToken);

            return result;

        }

 
        public async Task<Result<List<ClientSecretDto>>> ListClientSecrets(int clientId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(clientId);

            if (result.IsFailure)
            {
                return new Result<List<ClientSecretDto>>(result.Exception);
            }

            return result.Value.ClientSecrets;
        }
        public async Task<Result<List<ClientClaimDto>>> ListClaims(int clientId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(clientId);

            if (result.IsFailure)
            {
                return new Result<List<ClientClaimDto>>(result.Exception);
            }

            return result.Value.Claims ;
        }

        public async Task<Result<ClientClaimDto>> GetClaim(int clientId, int claimId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(clientId);

            if (result.IsFailure)
            {
                return new Result<ClientClaimDto>(result.Exception);
            }

            var claim = result.Value.Claims.SingleOrDefault(x => x.Id == claimId);

            if(claim == null)
            {
                return new Result<ClientClaimDto>(new EntityNotFoundException(typeof(ClientClaim), claimId));
            }

            return claim;
        }

        public async Task<Result<List<ClientPropertyDto>>> ListProperties(int clientId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(clientId);

            if (result.IsFailure)
            {
                return new Result<List<ClientPropertyDto>>(result.Exception);
            }

            return result.Value.Properties;
        }

        public async Task<Result<ClientPropertyDto>> GetProperty(int clientId, int propertId, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(clientId);

            if (result.IsFailure)
            {
                return new Result<ClientPropertyDto>(result.Exception);
            }

            var property = result.Value.Properties.SingleOrDefault(x => x.Id == propertId);

            if (property == null)
            {
                return new Result<ClientPropertyDto>(new EntityNotFoundException(typeof(ClientProperty), propertId));
            }

            return property;
        }


    }
}
