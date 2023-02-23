using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using System.Net;

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
        public async Task<IActionResult> GetClientList([FromQuery]PagingAndSortingParamsQueryString @params)
        {
            var queryParams = new PagingQueryParams { PageNumber = @params.PageNumber, PageSize = @params.PageSize };

            var result = await _clientQueryService.ListAsync(queryParams);

            return FromResult(result , HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("{clientId}")]
        public async Task<IActionResult> GetClientById(int clientId)
        {
            var result = await _clientQueryService.GetAsync(clientId);

            return FromResult(result, HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("")]
        public  async Task<IActionResult> CreateClient([FromBody] ClientModel model)
        {
            var result = await _clientCommandService.CreateAsync(model);

            return FromResult(result, HttpStatusCode.Created);
        }


        [HttpPut]
        [Route("{clientId}")]
        public async Task<IActionResult> UpdateClient(int clientId , [FromBody] ClientModel model)
        {
            var result = await _clientCommandService.UpdateAsync(clientId,model);

            return FromResult(result, HttpStatusCode.OK);
        }


        [HttpDelete]
        [Route("{clientId}")]
        public async Task<IActionResult> DeleteClient(int clientId)
        {
            var result = await _clientCommandService.DeleteAsync(clientId);

            return FromResult(result, HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("{clientId}/secrets")]
        public async Task<IActionResult> CreateClientSecret(int clientId, [FromBody] SecretModel model)
        {
            var result = await _clientCommandService.AddClientSecret(clientId,model);

            return FromResult(result, HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("{clientId}/secrets")]
        public async Task<IActionResult> CreateClientSecret(int clientId, int secretId)
        {
            var result = await _clientCommandService.DeleteClientSecret(clientId, secretId);

            return FromResult(result, HttpStatusCode.NoContent);
        }
    }
}
