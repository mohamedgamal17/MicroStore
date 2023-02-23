using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using System.Net;

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
        public async Task<IActionResult> GetListApiResource([FromQuery]PagingParamsQueryString @params)
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
        [Route("{apiResourceId}")]
        public async Task<IActionResult> GetApiResource(int apiResourceId)
        {
            var result = await _apiResourceQueryService.GetAsync(apiResourceId);

            return FromResultV2(result, HttpStatusCode.OK);
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateApiResource([FromBody] ApiResourceModel model)
        {
            var result = await _apiResourceCommandService.CreateAsync(model);

            return FromResultV2(result, HttpStatusCode.Created);
        }


        [HttpPut]
        [Route("{apiResourceId}")]
        public async Task<IActionResult> UpdateApiResource(int apiResourceId , [FromBody] ApiResourceModel model)
        {
            var result = await _apiResourceCommandService.UpdateAsync(apiResourceId,model);

            return FromResultV2(result, HttpStatusCode.OK);
        }



        [HttpDelete]
        [Route("{apiResourceId}")]

        public async Task<IActionResult> DeleteApiReosurce(int apiResourceId)
        {
            var result = await _apiResourceCommandService.DeleteAsync(apiResourceId);

            return FromResultV2(result, HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("{apiResourceId}/secrets")]
        public async Task<IActionResult> CreateApiResourceSecret(int apiResourceId, [FromBody] SecretModel model)
        {
            var result = await _apiResourceCommandService.AddApiSecret(apiResourceId, model);

            return FromResultV2(result, HttpStatusCode.OK);
        }


        [HttpDelete]
        [Route("{apiResourceId}/secrets/{secretId}")]
        public async Task<IActionResult> DeleteApiResourceSecret(int apiResourceId, int secretId)
        {
            var result = await _apiResourceCommandService.RemoveApiSecret(apiResourceId, secretId);

            return FromResultV2(result, HttpStatusCode.NoContent);
        }



    }
}
