using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Host.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;

namespace MicroStore.IdentityProvider.Host.Controllers
{
    [ApiController]
    [Route("api/apiscopes")]
    public class ApiScopeController : MicroStoreApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetListApiScope([FromQuery]PagingQueryParams @params)
        {
            var query = new GetApiScopeListQuery
            {
                PageNumber = @params.PageNumber,
                PageSize = @params.PageSize,
            };

            var result = await Send(query);

            return FromResult(result);
        }


        [HttpGet]
        [Route("{apiScopeId}")]
        public async Task<IActionResult> GetApiScope(int apiScopeId)
        {
            var query = new GetApiScopeQuery { ApiScopeId = apiScopeId };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateApiScope([FromBody]ApiScopeModel model)
        {
            var command = ObjectMapper.Map<ApiScopeModel, CreateApiResourceCommand>(model);

            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPut]
        [Route("{apiScopeId}")]
        public  async Task<IActionResult> UpdateApiScope(int apiScopeId, [FromBody] ApiScopeModel model)
        {
            var command = ObjectMapper.Map<ApiScopeModel, UpdateApiResourceCommand>(model);

            command.ApiResourceId = apiScopeId;

            var result = await Send(command);

            return FromResult(result);
        }


        [HttpDelete]
        [Route("{apiScopeId}")]
        public async Task<IActionResult> RemoveApiScope(int apiScopeId)
        {
            var command = new RemoveApiResourceCommand { ApiResourceId= apiScopeId };

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
