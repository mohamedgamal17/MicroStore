using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using System.Net;

namespace MicroStore.IdentityProvider.Host.Controllers
{
    [ApiController]
    [Route("api/apiscopes")]
    public class ApiScopeController : MicroStoreApiController
    {
        private readonly IApiScopeCommandService _apiScopeCommandService;

        private readonly IApiResourceQueryService _apiResourceQueryService;

        public ApiScopeController(IApiScopeCommandService apiScopeCommandService, IApiResourceQueryService apiResourceQueryService)
        {
            _apiScopeCommandService = apiScopeCommandService;
            _apiResourceQueryService = apiResourceQueryService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetListApiScope([FromQuery]PagingParamsQueryString @params)
        {
            var queryParams = new PagingQueryParams
            {
                PageNumber = @params.PageNumber,
                PageSize = @params.PageSize,
            };

            var result = await _apiResourceQueryService.ListAsync(queryParams);

            return FromResultV2(result, HttpStatusCode.OK);
        }


        [HttpGet]
        [Route("{apiScopeId}")]
        public async Task<IActionResult> GetApiScope(int apiScopeId)
        {

            var result = await _apiResourceQueryService.GetAsync(apiScopeId);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateApiScope([FromBody]ApiScopeModel model)
        {
            var result = await _apiScopeCommandService.CreateAsync(model);

            return FromResultV2(result, HttpStatusCode.Created);
        }

        [HttpPut]
        [Route("{apiScopeId}")]
        public  async Task<IActionResult> UpdateApiScope(int apiScopeId, [FromBody] ApiScopeModel model)
        {
            var result = await _apiScopeCommandService.UpdateAsync(apiScopeId, model);

            return FromResultV2(result, HttpStatusCode.OK);
        }


        [HttpDelete]
        [Route("{apiScopeId}")]
        public async Task<IActionResult> RemoveApiScope(int apiScopeId)
        {
            var result = await _apiScopeCommandService.DeleteAsync(apiScopeId);

            return FromResultV2(result, HttpStatusCode.NoContent);
        }
    }
}
