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
        public async Task<UnitResultV2<ClientDto>> CreateAsync(ClientModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateClient(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResultV2.Failure<ClientDto>(validationResult.Error);
            }

            var client = ObjectMapper.Map<ClientModel, Client>(model);

            await _clinetRepository.InsertAsync(client);

            return UnitResultV2.Success(ObjectMapper.Map<Client, ClientDto>(client));
        }
        public async Task<UnitResultV2<ClientDto>> UpdateAsync(int clientId, ClientModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return UnitResultV2.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet with id : {clientId} is not exist"));
            }

            var validationResult = await ValidateClient(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResultV2.Failure<ClientDto>(validationResult.Error);
            }

            client = ObjectMapper.Map(model,client);

            await _clinetRepository.UpdateClinetAsync(client, cancellationToken);

            return UnitResultV2.Success(ObjectMapper.Map<Client, ClientDto>(client));
        }

        public async Task<UnitResultV2> DeleteAsync(int clientId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
              , cancellationToken);

            if (client == null)
            {
                return UnitResultV2.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet with id : {clientId} is not exist"));
            }

            await _clinetRepository.DeleteAsync(client);

            return UnitResultV2.Success();
        }

        public async Task<UnitResultV2<ClientDto>> AddClientSecret(int clientId, SecretModel model, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return UnitResultV2.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet with id : {clientId} is not exist"));
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

            return UnitResultV2.Success(ObjectMapper.Map<Client, ClientDto>(client));
        }


        public async Task<UnitResultV2<ClientDto>> DeleteClientSecret(int clientId, int secretId, CancellationToken cancellationToken = default)
        {
            var client = await _clinetRepository.SingleOrDefaultAsync(x => x.Id == clientId
             , cancellationToken);

            if (client == null)
            {
                return UnitResultV2.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet with id : {clientId} is not exist"));
            }

            if (client.ClientSecrets == null || !client.ClientSecrets.Any(x => x.Id == secretId))
            {
                return UnitResultV2.Failure<ClientDto>(ErrorInfo.NotFound($"Clinet dose not have secret id : {secretId}"));
            }

            var secret = client.ClientSecrets.Single(x => x.Id == secretId);


            client.ClientSecrets.Remove(secret);

            await _clinetRepository.UpdateAsync(client, cancellationToken: cancellationToken);

            return UnitResultV2.Success(ObjectMapper.Map<Client, ClientDto>(client));
        }

        private async Task<UnitResultV2> ValidateClient(ClientModel model , int? clientId=  null , CancellationToken cancellationToken = default)
        {
            var query = _clinetRepository.Query();

            if(clientId != null)
            {
                query = query.Where(x => x.Id != clientId);
            }

            if(await _clinetRepository.AnyAsync(x=> x.ClientId == model.ClientId))
            {
                return UnitResultV2.Failure(ErrorInfo.BusinessLogic($"Clinet id : {model.ClientId} is already exist"));
            }

            return UnitResultV2.Success();
        }
    }
}
