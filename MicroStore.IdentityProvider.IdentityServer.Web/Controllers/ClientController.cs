using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
namespace MicroStore.IdentityProvider.Host.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : MicroStoreApiController
    {

        private readonly IClientCommandService _clientCommandService;

        private readonly IClientQueryService _clientQueryService;

        public ClientController(IClientCommandService clientCommandService, IClientQueryService clientQueryService)
        {
            _clientCommandService = clientCommandService;
            _clientQueryService = clientQueryService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetClientList([FromQuery] ClientListQueryModel queryParams)
        {
            var result = await _clientQueryService.ListAsync(queryParams);

            return result.ToOk();
        }

        [HttpGet]
        [ActionName(nameof(GetClientById))]
        [Route("{clientId}")]
        public async Task<IActionResult> GetClientById(int clientId)
        {
            var result = await _clientQueryService.GetAsync(clientId);

            return result.ToOk();
        }

        [HttpPost]
        [Route("")]
        public  async Task<IActionResult> CreateClient([FromBody] ClientModel model)
        {
            var result = await _clientCommandService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(GetClientById), new { clientId = result.Value?.Id });
        }


        [HttpPut]
        [Route("{clientId}")]
        public async Task<IActionResult> UpdateClient(int clientId , [FromBody] ClientModel model)
        {
            var result = await _clientCommandService.UpdateAsync(clientId,model);

            return result.ToOk();
        }


        [HttpDelete]
        [Route("{clientId}")]
        public async Task<IActionResult> DeleteClient(int clientId)
        {
            var result = await _clientCommandService.DeleteAsync(clientId);

            return result.ToNoContent();
        }

        [HttpPost]
        [Route("{clientId}/secrets")]
        public async Task<IActionResult> CreateClientSecret(int clientId, [FromBody] SecretModel model)
        {
            var result = await _clientCommandService.AddClientSecret(clientId,model);

            return result.ToOk();
        }

        [HttpDelete]
        [Route("{clientId}/secrets")]
        public async Task<IActionResult> RemoveClientSecret(int clientId, int secretId)
        {
            var result = await _clientCommandService.DeleteClientSecret(clientId, secretId);

            return result.ToNoContent();
        }


    }
}
