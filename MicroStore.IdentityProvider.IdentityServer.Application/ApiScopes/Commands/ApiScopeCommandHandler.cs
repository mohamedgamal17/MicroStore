using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using System.Net;
using System.Xml.Linq;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes.Commands
{
    public class ApiScopeCommandHandler : RequestHandler,
        ICommandHandler<CreateApiScopeCommand, ApiScopeDto>,
        ICommandHandler<UpdateApiScopeCommand, ApiScopeDto>,
        ICommandHandler<RemoveApiScopeCommand>
    {

        private readonly IApiScopeRepository _apiScopeRepository;

        public ApiScopeCommandHandler(IApiScopeRepository apiScopeRepository)
        {
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<ResponseResult<ApiScopeDto>> Handle(CreateApiScopeCommand request, CancellationToken cancellationToken)
        {
            var apiScope = new ApiScope();

            PrepareApiScopeEntity(request, apiScope);

            await _apiScopeRepository.InsertAsync(apiScope, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.Created, ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope));
        }



        public async Task<ResponseResult<ApiScopeDto>> Handle(UpdateApiScopeCommand request, CancellationToken cancellationToken)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == request.ApiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return Failure<ApiScopeDto>(HttpStatusCode.NotFound, $"Api Scope with id : {request.ApiScopeId} is not exist");
            }

            PrepareApiScopeEntity(request, apiScope);

            await _apiScopeRepository.UpdateApiScopeAsync(apiScope, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope));
        }

        public async Task<ResponseResult<Unit>> Handle(RemoveApiScopeCommand request, CancellationToken cancellationToken)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == request.ApiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Api Scope with id : {request.ApiScopeId} is not exist");
            }

            await _apiScopeRepository.DeleteAsync(apiScope, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.NoContent);
        }

        private void PrepareApiScopeEntity(ApiScopeCommand request, ApiScope apiScope)
        {
            apiScope.Name = request.Name;
            apiScope.DisplayName = request.DisplayName ?? string.Empty;
            apiScope.Description = request.Description ?? string.Empty;
            apiScope.ShowInDiscoveryDocument = request.ShowInDiscoveryDocument;
            apiScope.Emphasize = request.Emphasize;
            apiScope.UserClaims = request.UserClaims?.Select(x => new ApiScopeClaim { Type = x }).ToList() ?? new List<ApiScopeClaim>();
            apiScope.Properties = request.Properties?.Select(x => new ApiScopeProperty { Key = x.Key, Value = x.Value }).ToList() ??
                 new List<ApiScopeProperty>();
        }
    }
}
