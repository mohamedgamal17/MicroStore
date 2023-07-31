using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
namespace MicroStore.IdentityProvider.Host.Controllers
{
    [ApiController]
    [Route("api/apiscopes")]
    public class ApiScopeController : MicroStoreApiController
    {
        private readonly IApiScopeCommandService _apiScopeCommandService;

        private readonly IApiScopeQueryService _apiScopeQueryService;

        public ApiScopeController(IApiScopeCommandService apiScopeCommandService, IApiScopeQueryService apiResourceQueryService)
        {
            _apiScopeCommandService = apiScopeCommandService;
            _apiScopeQueryService = apiResourceQueryService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetListApiScope()
        {
            var result = await _apiScopeQueryService.ListAsync();

            return result.ToOk();
        }


        [HttpGet]
        [ActionName(nameof(GetApiScope))]
        [Route("{apiScopeId}")]
        public async Task<IActionResult> GetApiScope(int apiScopeId)
        {

            var result = await _apiScopeQueryService.GetAsync(apiScopeId);

            return result.ToOk();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateApiScope([FromBody]ApiScopeModel model)
        {
            var result = await _apiScopeCommandService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(GetApiScope), new { apiScopeId = result.Value?.Id });
        }

        [HttpPut]
        [Route("{apiScopeId}")]
        public  async Task<IActionResult> UpdateApiScope(int apiScopeId, [FromBody] ApiScopeModel model)
        {
            var result = await _apiScopeCommandService.UpdateAsync(apiScopeId, model);

            return result.ToOk();
        }


        [HttpDelete]
        [Route("{apiScopeId}")]
        public async Task<IActionResult> RemoveApiScope(int apiScopeId)
        {
            var result = await _apiScopeCommandService.DeleteAsync(apiScopeId);

            return result.ToNoContent();
        }
    }
}
