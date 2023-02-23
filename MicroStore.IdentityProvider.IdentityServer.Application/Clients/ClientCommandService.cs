using Duende.IdentityServer.EntityFramework.Entities;
using IdentityModel;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
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
        public async Task<UnitResult<ClientDto>> CreateAsync(ClientModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateClient(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<ClientDto>(validationResult.Error);
            }

            var client = ObjectMapper.Map<ClientModel, Client>(model);

            await _clinetRepository.InsertAsync(client);

            return UnitResult.Success(ObjectMapper.Map<Client, ClientDto>(client));
        }
        public async Task<UnitResult<ClientDto>> UpdateAsync(int clientId, ClientModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return UnitResult.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet with id : {clientId} is not exist"));
            }

            var validationResult = await ValidateClient(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<ClientDto>(validationResult.Error);
            }

            client = ObjectMapper.Map(model,client);

            await _clinetRepository.UpdateClinetAsync(client, cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<Client, ClientDto>(client));
        }

        public async Task<UnitResult> DeleteAsync(int clientId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
              , cancellationToken);

            if (client == null)
            {
                return UnitResult.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet with id : {clientId} is not exist"));
            }

            await _clinetRepository.DeleteAsync(client);

            return UnitResult.Success();
        }

        public async Task<UnitResult<ClientDto>> AddClientSecret(int clientId, SecretModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return UnitResult.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet with id : {clientId} is not exist"));
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

            return UnitResult.Success(ObjectMapper.Map<Client, ClientDto>(client));
        }


        public async Task<UnitResult<ClientDto>> DeleteClientSecret(int clientId, int secretId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return UnitResult.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet with id : {clientId} is not exist"));
            }

            if (client.ClientSecrets == null || !client.ClientSecrets.Any(x => x.Id == secretId))
            {
                return UnitResult.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet dose not have secret id : {secretId}"));
            }

            var secret = client.ClientSecrets.Single(x => x.Id == secretId);


            client.ClientSecrets.Remove(secret);

            await _clinetRepository.UpdateAsync(client, cancellationToken: cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<Client, ClientDto>(client));
        }

        private async Task<UnitResult> ValidateClient(ClientModel model , int? clientId=  null , CancellationToken cancellationToken = default)
        {
            var query = _clinetRepository.Query();

            if(clientId != null)
            {
                query = query.Where(x => x.Id != clientId);
            }

            if(await _clinetRepository.AnyAsync(x=> x.ClientId == model.ClientId))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"Clinet id : {model.ClientId} is already exist"));
            }

            return UnitResult.Success();
        }
    }
}
