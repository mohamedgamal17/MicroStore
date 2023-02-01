using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using System.Net;
using static IdentityModel.OidcConstants;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients.Commands
{
    public class ClientCommandHandler : RequestHandler ,
        ICommandHandler<CreateClientCommand, ClientDto>,
        ICommandHandler<UpdateClientCommand , ClientDto>,
        ICommandHandler<RemoveClientCommand>,
        ICommandHandler<AddClientSecretCommand , ClientDto>,
        ICommandHandler<RemoveClientSecretCommand, ClientDto>
    {
        const string SecretType = "SharedSecret";

        const int ClinetIdLenght = 16;

        const string ClinetIdPerfix = "microstoreclinet";

        private readonly IClinetRepository _clinetRepository;

        private readonly ICryptoServiceProvider _cryptoServiceProvider;

        public ClientCommandHandler(IClinetRepository clinetRepository, ICryptoServiceProvider cryptoServiceProvider)
        {
            _clinetRepository = clinetRepository;
            _cryptoServiceProvider = cryptoServiceProvider;
        }
        public async Task<ResponseResult<ClientDto>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var clinet = new Client();

            PrepareClientEntity(request, clinet);

            clinet.ClientId = await _cryptoServiceProvider.GenerateRandomEncodedBase64Key(ClinetIdLenght, ClinetIdPerfix);

            await _clinetRepository.InsertAsync(clinet, cancellationToken : cancellationToken);

            return Success(HttpStatusCode.Created, ObjectMapper.Map<Client,ClientDto>(clinet));

        }

        public async Task<ResponseResult<ClientDto>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == request.ClientId
             , cancellationToken);

            if (client == null)
            {
                return Failure<ClientDto>(HttpStatusCode.NotFound, $"Clinet with id : {request.ClientId} is not exist");
            }

            PrepareClientEntity(request, client);

           await _clinetRepository.UpdateAsync(client,cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<Client, ClientDto>(client));

        }

        public async Task<ResponseResult<Unit>> Handle(RemoveClientCommand request, CancellationToken cancellationToken)
        {
            var clinet = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == request.ClinetId,cancellationToken);

            if(clinet == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Clinet with id : {request.ClinetId} is not exist");
            }



            await _clinetRepository.DeleteAsync(clinet, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseResult<ClientDto>> Handle(AddClientSecretCommand request, CancellationToken cancellationToken)
        {
            var clinet = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == request.ClinetId, cancellationToken);

            if (clinet == null)
            {
                return Failure<ClientDto>(HttpStatusCode.NotFound, $"Clinet with id : {request.ClinetId} is not exist");
            }

            if(clinet.ClientSecrets == null)
            {
                clinet.ClientSecrets = new List<ClientSecret>();
            }

            clinet.ClientSecrets.Add( new ClientSecret
            {
                Type = SecretType,

                Value = request.ResolveApiResourceSecret(),

                Description =request.Description

            });

            await _clinetRepository.UpdateAsync(clinet, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<Client, ClientDto>(clinet));
        }

        public async Task<ResponseResult<ClientDto>> Handle(RemoveClientSecretCommand request, CancellationToken cancellationToken)
        {
            var clinet = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == request.ClinetId, cancellationToken);

            if (clinet == null)
            {
                return Failure<ClientDto>(HttpStatusCode.NotFound, $"Clinet with id : {request.ClinetId} is not exist");
            }

            if(clinet.ClientSecrets == null || !clinet.ClientSecrets.Any(x => x.Id == request.SecretId))
            {
                return Failure<ClientDto>(HttpStatusCode.BadRequest, $"Clinet dose not have secret id : {request.SecretId}");
            }

            var secret = clinet.ClientSecrets.Single(x => x.Id == request.SecretId);

            clinet.ClientSecrets.Remove(secret);

            await _clinetRepository.UpdateAsync(clinet, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<Client, ClientDto>(clinet));
        }



        private void PrepareClientEntity(ClientCommand request , Client client)
        {
            client.ClientName = request.ClientName;
            client.ClientUri = request.ClientUri;
            client.Description = request.Description ?? string.Empty;
            client.LogoUri = request.LogoUri ?? string.Empty;
            client.RedirectUris = request.AllowedRedirectUris?.Select(x => new ClientRedirectUri { RedirectUri = x }).ToList() ?? new List<ClientRedirectUri>();
                client.PostLogoutRedirectUris = request.PostLogoutRedirectUris?.Select(x => new ClientPostLogoutRedirectUri { PostLogoutRedirectUri = x }).ToList() ??
            new List<ClientPostLogoutRedirectUri>();
            client.AllowRememberConsent = request.AllowRememberConsent;
            client.RequireConsent = request.RequireConsent;
            client.ConsentLifetime = request.ConsentLifetime;
            client.FrontChannelLogoutUri = request.FrontChannelLogoutUri;
            client.FrontChannelLogoutSessionRequired = request.FrontChannelLogoutSessionRequired;
            client.BackChannelLogoutUri = request.BackChannelLogoutUri;
            client.BackChannelLogoutSessionRequired = request.BackChannelLogoutSessionRequired;
            client.AlwaysIncludeUserClaimsInIdToken = request.AlwaysIncludeUserClaimsInIdToken;
            client.IdentityTokenLifetime = request.IdentityTokenLifetime;
            client.AccessTokenLifetime = request.AccessTokenLifetime;
            client.AccessTokenType = (int)request.AccessTokenType;
            client.UpdateAccessTokenClaimsOnRefresh = request.UpdateAccessTokenClaimsOnRefresh;
            client.RefreshTokenExpiration = (int)request.RefreshTokenExpiration;
            client.RefreshTokenUsage = (int)request.RefreshTokenUsage;
            client.SlidingRefreshTokenLifetime = request.SlidingRefreshTokenLifetime;
            client.AbsoluteRefreshTokenLifetime = request.AbsoluteRefreshTokenLifetime;
            client.AllowOfflineAccess = request.AllowOfflineAccess;
            client.RequireClientSecret = request.RequireClientSecret;
            client.RequirePkce = request.RequirePkce;
            client.AllowedCorsOrigins = request.AllowedCorsOrigins?.Select(x => new ClientCorsOrigin { Origin = x }).ToList() ?? new List<ClientCorsOrigin>();
            client.AllowedGrantTypes = request.AllowedGrantTypes?.Select(x => new ClientGrantType { GrantType = x }).ToList() ?? new List<ClientGrantType>();
            client.AllowedScopes = request.AllowedScopes?.Select(x => new ClientScope { Scope = x }).ToList() ?? new List<ClientScope>();
            client.Properties = request.Properties?.Select(x => new ClientProperty { Key = x.Key, Value = x.Value }).ToList() ?? new List<ClientProperty>();
        }
    
    }
}
