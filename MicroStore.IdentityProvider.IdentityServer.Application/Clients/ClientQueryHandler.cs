using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public class ClientQueryHandler : RequestHandler,
        IQueryHandler<GetClientListQuery, PagedResult<ClientDto>>,
        IQueryHandler<GetClientQuery, ClientDto>
    {
        private readonly IApplicationConfigurationDbContext _applicationConfigurationDbContext;

        public ClientQueryHandler(IApplicationConfigurationDbContext applicationConfigurationDbContext)
        {
            _applicationConfigurationDbContext = applicationConfigurationDbContext;
        }

        public async Task<ResponseResult<PagedResult<ClientDto>>> Handle(GetClientListQuery request, CancellationToken cancellationToken)
        {
            var query = _applicationConfigurationDbContext.Clients.AsNoTracking().ProjectTo<ClientDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<ClientDto>> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            var query = _applicationConfigurationDbContext.Clients.AsNoTracking().ProjectTo<ClientDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.ClientId, cancellationToken);

            if (result == null)
            {
                return Failure<ClientDto>(HttpStatusCode.NotFound, $"Clinet with id : {request.ClientId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
