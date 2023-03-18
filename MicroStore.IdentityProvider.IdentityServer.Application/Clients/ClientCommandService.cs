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
        private readonly IClientRepository _clinetRepository;

        const string SecretType = "SharedSecret";

        public ClientCommandService(IClientRepository clinetRepository)
        {
            _clinetRepository = clinetRepository;
        }
        public async Task<ResultV2<ClientDto>> CreateAsync(ClientModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateClient(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new ResultV2<ClientDto>(validationResult.Exception);
            }

            var client = ObjectMapper.Map<ClientModel, Client>(model);

            await _clinetRepository.InsertAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }
        public async Task<ResultV2<ClientDto>> UpdateAsync(int clientId, ClientModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return new ResultV2<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            var validationResult = await ValidateClient(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new ResultV2<ClientDto>(validationResult.Exception);
            }

            client = ObjectMapper.Map(model,client);

            await _clinetRepository.UpdateClinetAsync(client, cancellationToken);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task<ResultV2<Unit>> DeleteAsync(int clientId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
              , cancellationToken);

            if (client == null)
            {
                return new ResultV2<Unit>(new EntityNotFoundException(typeof(Client), clientId));
            }

            await _clinetRepository.DeleteAsync(client);

            return Unit.Value;
        }

        public async Task<ResultV2<ClientDto>> AddClientSecret(int clientId, SecretModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return new ResultV2<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
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


        public async Task<ResultV2<ClientDto>> DeleteClientSecret(int clientId, int secretId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return new ResultV2<ClientDto>(new EntityNotFoundException(typeof(Client), clientId));
            }

            if (client.ClientSecrets == null || !client.ClientSecrets.Any(x => x.Id == secretId))
            {
                return new ResultV2<ClientDto>(new EntityNotFoundException(typeof(ClientSecret), secretId));
            }

            var secret = client.ClientSecrets.Single(x => x.Id == secretId);


            client.ClientSecrets.Remove(secret);

            await _clinetRepository.UpdateAsync(client, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        private async Task<ResultV2<Unit>> ValidateClient(ClientModel model , int? clientId=  null , CancellationToken cancellationToken = default)
        {
            var query = _clinetRepository.Query();

            if(clientId != null)
            {
                query = query.Where(x => x.Id != clientId);
            }

            if(await _clinetRepository.AnyAsync(x=> x.ClientId == model.ClientId))
            {
                return new ResultV2<Unit>(new BusinessException($"Clinet id : {model.ClientId} is already exist"));
            }

            return Unit.Value;
        }
    }
}
