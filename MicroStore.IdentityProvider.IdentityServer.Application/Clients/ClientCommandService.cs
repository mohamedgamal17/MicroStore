using Duende.IdentityServer.EntityFramework.Entities;
using IdentityModel;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public class ClientCommandService : IdentityServiceApplicationService, IClientCommandService
    {
        private readonly IRepository<Client> _clinetRepository;

        const string SecretType = "SharedSecret";

        public ClientCommandService(IRepository<Client> clinetRepository)
        {
            _clinetRepository = clinetRepository;
        }
        public async Task<Result<ClientDto>> CreateAsync(ClientModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateClient(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ClientDto>(validationResult.Exception);
            }

            var client = ObjectMapper.Map<ClientModel, Client>(model);

            await _clinetRepository.InsertAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }
        public async Task<Result<ClientDto>> UpdateAsync(int clientId, ClientModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            var validationResult = await ValidateClient(model, clientId, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ClientDto>(validationResult.Exception);
            }

            client = ObjectMapper.Map(model, client);

            await _clinetRepository.UpdateAsync(client, cancellationToken);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task<Result<Unit>> DeleteAsync(int clientId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
              , cancellationToken);

            if (client == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(Client), clientId));
            }

            await _clinetRepository.DeleteAsync(client);

            return Unit.Value;
        }

        public async Task<Result<ClientDto>> AddClientSecret(int clientId, SecretModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            if (client.ClientSecrets == null)
            {
                client.ClientSecrets = new List<ClientSecret>();
            }


            client.ClientSecrets.Add(new ClientSecret
            {
                Type = SecretType,

                Value = model.Value.ToSha512(),

                Description = model.Description

            });

            await _clinetRepository.UpdateAsync(client, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }


        public async Task<Result<ClientDto>> DeleteClientSecret(int clientId, int secretId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            if (client.ClientSecrets == null || !client.ClientSecrets.Any(x => x.Id == secretId))
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(ClientSecret), secretId));
            }

            var secret = client.ClientSecrets.Single(x => x.Id == secretId);


            client.ClientSecrets.Remove(secret);

            await _clinetRepository.UpdateAsync(client, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        private async Task<Result<Unit>> ValidateClient(ClientModel model, int? clientId = null, CancellationToken cancellationToken = default)
        {
            var query = _clinetRepository.Query();

            if (clientId != null)
            {
                query = query.Where(x => x.Id != clientId);
            }

            if (query.Any(x => x.ClientId.ToUpper() == model.ClientId.ToUpper()))
            {
                return new Result<Unit>(new UserFriendlyException($"Clinet id : {model.ClientId} is already exist"));
            }

            return Unit.Value;
        }

        public async Task<Result<ClientDto>> AddClaim(int clientId, ClaimModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId, cancellationToken);

            if (client == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            if (client.Claims == null)
            {
                client.Claims = new List<ClientClaim>();
            }

            var validationResult = await ValidateClaim(clientId, model, cancellationToken : cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ClientDto>(validationResult.Exception);
            }

            var claim = ObjectMapper.Map<ClaimModel, ClientClaim>(model);

            client.Claims.Add(claim);

            await _clinetRepository.UpdateAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task<Result<ClientDto>> UpdateClaim(int clientId, int claimId, ClaimModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId, cancellationToken);

            if (client == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            var claim = client.Claims?.SingleOrDefault(x => x.Id == claimId);

            if (claim == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(ClientClaim), claimId));
            }

            var validationResult = await ValidateClaim(clientId, model, claimId, cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ClientDto>(validationResult.Exception);
            }

            ObjectMapper.Map(model, claim);

            await _clinetRepository.UpdateAsync(client, cancellationToken);


            return ObjectMapper.Map<Client, ClientDto>(client);

        }

        public async Task<Result<ClientDto>> RemoveClaim(int clientId, int claimId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId, cancellationToken);

            if (client == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            var claim = client.Claims?.SingleOrDefault(x => x.Id == claimId);

            if (claim == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(ClientClaim), claimId));
            }

            client.Claims!.Remove(claim);

            await _clinetRepository.UpdateAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task<Result<ClientDto>> AddProperty(int clientId, PropertyModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId, cancellationToken);

            if (client == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            if (client.Properties == null)
            {
                client.Properties = new List<ClientProperty>();
            }

            var validationResult =  await ValidateProperty(clientId, model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ClientDto>(validationResult.Exception);
            }

            var property = ObjectMapper.Map<PropertyModel, ClientProperty>(model);

            client.Properties.Add(property);

            await _clinetRepository.UpdateAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task<Result<ClientDto>> UpdateProperty(int clientId, int propertyId, PropertyModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId, cancellationToken);

            if (client == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            var property = client.Properties?.SingleOrDefault(x => x.Id == propertyId);

            if (property == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(ClientProperty), propertyId));
            }

            var validationResult = await ValidateProperty(clientId, model, propertyId, cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ClientDto>(validationResult.Exception);
            }

            ObjectMapper.Map(model, property);

            await _clinetRepository.UpdateAsync(client, cancellationToken);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task<Result<ClientDto>> RemoveProperty(int clientId, int propertyId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId, cancellationToken);

            if (client == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            var property = client.Properties?.SingleOrDefault(x => x.Id == propertyId);

            if (property == null)
            {
                return new Result<ClientDto>(new EntityNotFoundException(typeof(ClientProperty), propertyId));
            }

            client.Properties!.Remove(property);

            await _clinetRepository.UpdateAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        private async Task<Result<Unit>> ValidateClaim(int clientId, ClaimModel model, int? claimId = null, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleAsync(x => x.Id == clientId, cancellationToken);

            var claim = client.Claims.WhereIf(claimId != null, (x) => x.Id != claimId).ToList();

            if (claim.Any(x => x.Type.ToLower() == model.Type.ToLower() && x.Value.ToLower() == model.Value.ToLower()))
            {
                return new Result<Unit>(new UserFriendlyException($"There is claim with type {model.Type} and value {model.Value}"));
            }

            return new Result<Unit>(Unit.Value);
        }

        private async Task<Result<Unit>> ValidateProperty(int clientId, PropertyModel model, int? propertyId = null, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleAsync(x => x.Id == clientId, cancellationToken);

            var properties = client.Properties.WhereIf(propertyId != null, (x) => x.Id != propertyId).ToList();

            if (properties.Any(x => x.Key.ToUpper() == model.Key.ToUpper()))
            {
                return new Result<Unit>(new UserFriendlyException($"There is already property with key {model.Key}"));
            }

            return new Result<Unit>(Unit.Value);
        }
    }
}
