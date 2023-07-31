using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
namespace MicroStore.IdentityProvider.Host.Controllers
{
    [ApiController]
    [Route("api/apiresources")]
    public class ApiResourceController : MicroStoreApiController
    {
        private readonly IApiResourceCommandService _apiResourceCommandService;

        private readonly IApiResourceQueryService _apiResourceQueryService;

        public ApiResourceController(IApiResourceCommandService apiResourceCommandService, IApiResourceQueryService apiResourceQueryService)
        {
            _apiResourceCommandService = apiResourceCommandService;
            _apiResourceQueryService = apiResourceQueryService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetListApiResource([FromQuery] ApiResourceListQueryModel queryParams)
        {
            var result = await _apiResourceQueryService.ListAsync(queryParams);

            return result.ToOk();
        }

        [HttpGet]
        [ActionName(nameof(GetApiResource))]
        [Route("{apiResourceId}")]
        public async Task<IActionResult> GetApiResource(int apiResourceId)
        {
            var result = await _apiResourceQueryService.GetAsync(apiResourceId);

            return result.ToOk();
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateApiResource([FromBody] ApiResourceModel model)
        {
            var result = await _apiResourceCommandService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(GetApiResource), new { apiResourceId = result.Value?.Id });
        }


        [HttpPut]
        [Route("{apiResourceId}")]
        public async Task<IActionResult> UpdateApiResource(int apiResourceId , [FromBody] ApiResourceModel model)
        {
            var result = await _apiResourceCommandService.UpdateAsync(apiResourceId,model);

            return result.ToOk();
        }



        [HttpDelete]
        [Route("{apiResourceId}")]

        public async Task<IActionResult> DeleteApiReosurce(int apiResourceId)
        {
            var result = await _apiResourceCommandService.DeleteAsync(apiResourceId);

            return result.ToNoContent();
        }

        [HttpPost]
        [Route("{apiResourceId}/secrets")]
        public async Task<IActionResult> CreateApiResourceSecret(int apiResourceId, [FromBody] SecretModel model)
        {
            var result = await _apiResourceCommandService.AddSecret(apiResourceId, model);

            return result.ToOk();
        }


        [HttpDelete]
        [Route("{apiResourceId}/secrets/{secretId}")]
        public async Task<IActionResult> DeleteApiResourceSecret(int apiResourceId, int secretId)
        {
            var result = await _apiResourceCommandService.RemoveSecret(apiResourceId, secretId);

            return result.ToNoContent();
        }



    }
}
