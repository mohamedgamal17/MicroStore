using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Host.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;

namespace MicroStore.IdentityProvider.Host.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : MicroStoreApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetClientList([FromQuery]PagingQueryParams @params)
        {
            var query = new GetClientListQuery { PageNumber = @params.PageNumber, PageSize = @params.PageSize };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{clientId}")]
        public async Task<IActionResult> GetClientById(int clientId,[FromQuery] PagingQueryParams @params)
        {
            var query = new GetClientQuery { ClientId = clientId };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpPost]
        [Route("")]
        public  async Task<IActionResult> CreateClient([FromBody] ClientModel model)
        {
            var command = ObjectMapper.Map<ClientModel, CreateClientCommand>(model);

            var result = await Send(command);

            return FromResult(result);
        }


        [HttpPut]
        [Route("{clientId}")]
        public async Task<IActionResult> UpdateClient(int clientId , [FromBody] ClientModel model)
        {
            var command = ObjectMapper.Map<ClientModel,UpdateClientCommand>(model);

            var result = await Send(command);

            return FromResult(result);
        }


        [HttpDelete]
        [Route("{clientId}")]
        public async Task<IActionResult> DeleteClient(int clientId)
        {
            var command = new RemoveClientCommand { ClinetId = clientId };

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
