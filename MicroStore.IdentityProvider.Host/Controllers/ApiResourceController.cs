using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Host.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;

namespace MicroStore.IdentityProvider.Host.Controllers
{
    [ApiController]
    [Route("api/apiresources")]
    public class ApiResourceController : MicroStoreApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetListApiResource([FromQuery]PagingQueryParams @params)
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
        [Route("{apiResourceId}")]
        public async Task<IActionResult> GetApiResource(int apiResourceId)
        {
            var query = new GetApiResourceQuery { ApiResourceId = apiResourceId };

            var result = await Send(query);

            return FromResult(result);
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateApiResource([FromBody] ApiResourceModel model)
        {
            var command = ObjectMapper.Map<ApiResourceModel, CreateApiResourceCommand>(model);

            var result = await Send(command);

            return FromResult(result);
        }


        [HttpPut]
        [Route("{apiResourceId}")]
        public async Task<IActionResult> UpdateApiResource(int apiResourceId , [FromBody] ApiResourceModel model)
        {
            var command = ObjectMapper.Map<ApiResourceModel, UpdateApiResourceCommand>(model);

            command.ApiResourceId = apiResourceId;

            var result = await Send(command);

            return FromResult(result);
        }



        [HttpDelete]
        public async Task<IActionResult> DeleteApiReosurce(int apiResourceId)
        {
            var command = new RemoveApiResourceCommand { ApiResourceId = apiResourceId };

            var result = await Send(command);

            return FromResult(result);
        }





    }
}
