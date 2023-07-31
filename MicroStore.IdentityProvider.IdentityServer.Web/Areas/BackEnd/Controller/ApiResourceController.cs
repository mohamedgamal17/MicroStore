using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Controller
{
    public class ApiResourceController : BackEndController
    {
        private readonly IApiResourceCommandService _apiResourceCommandService;

        private readonly IApiResourceQueryService _apiResourceQueryService;

        private readonly IApiScopeQueryService _apiScopeQueryService;
        public ApiResourceController(IApiResourceCommandService apiResourceCommandService, IApiResourceQueryService apiResourceQueryService, IApiScopeQueryService apiScopeQueryService)
        {
            _apiResourceCommandService = apiResourceCommandService;
            _apiResourceQueryService = apiResourceQueryService;
            _apiScopeQueryService = apiScopeQueryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ApiResourceListUIModel model)
        {
            var pagingOptions = new PagingQueryParams { Length = model.PageSize, Skip = model.Skip };

            var result = await _apiResourceQueryService.ListAsync(pagingOptions);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            model.Data = result.Value.Items;

            return Json(model);
        }

        public async Task<IActionResult> Create()
        {
            return View(new ApiResourceUIModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApiResourceUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var apiResourceModel = ObjectMapper.Map<ApiResourceUIModel, ApiResourceModel>(model);

            var result = await _apiResourceCommandService.CreateAsync(apiResourceModel);

            if (result.IsFailure)
            {

                return HandleFailureResultWithView(result, model);
            }

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit(int id)
        {
            var result = await _apiResourceQueryService.GetAsync(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            var model = ObjectMapper.Map<ApiResourceDto, ApiResourceUIModel>(result.Value);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApiResourceUIModel model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            var apiResourceModel = ObjectMapper.Map<ApiResourceUIModel, ApiResourceModel>(model);

            var result = await _apiResourceCommandService.UpdateAsync(model.Id, apiResourceModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result, model);
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _apiResourceCommandService.DeleteAsync(id);


            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> ListApiResourceSecrets(int id, ApiResourceSecretListUIModel model)
        {
            var result = await _apiResourceQueryService.ListApiResourceSecrets(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            model.Data = result.Value;

            return Json(model);
        }



        public IActionResult CreateApiResourceSecretModel(int apiResourceId)
        {
            return PartialView("_Create.ApiResourceSecretModal", new ApiResourceSecretUIModel { ApiResourceId = apiResourceId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateApiResourceSecret(ApiResourceSecretUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);

            }

            var apiResourceSecretModel = ObjectMapper.Map<ApiResourceSecretUIModel, SecretModel>(model);

            var result = await _apiResourceCommandService.AddSecret(model.ApiResourceId, apiResourceSecretModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiResourceSecret([FromBody] RemoveApiResourceSecretUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await _apiResourceCommandService.RemoveSecret(model.ApiResourceId, model.ApiResourceSecretId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return NoContent();
        }



        [HttpPost]
        public async Task<IActionResult> ListApiResourceProperties(int id, ApiResourcePropertyListUIModel model)
        {
            var result = await _apiResourceQueryService.ListProperties(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }


            model.Data = result.Value;

            return Json(model);
        }

        public IActionResult CreatePropertyModal(int parentId)
        {
            return PartialView("_Create.Property", new PropertyViewModel { ParentId = parentId });
        }

        public async Task<IActionResult> EditPropertyModal(int parentId, int propertyId)
        {
            var result = await _apiResourceQueryService.GetApiResourceProperty(parentId, propertyId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            return PartialView("_Edit.Property", new PropertyViewModel
            {
                ParentId = result.Value.ApiResourceId,
                PropertyId = result.Value.Id,
                Key = result.Value.Key,
                Value = result.Value.Value
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProperty(PropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var propertyModel = ObjectMapper.Map<PropertyViewModel, PropertyModel>(model);

            var result = await _apiResourceCommandService.AddProperty(model.ParentId, propertyModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProperty(PropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var propertyModel = ObjectMapper.Map<PropertyViewModel, PropertyModel>(model);

            var result = await _apiResourceCommandService.UpdateProperty(model.ParentId, model.PropertyId, propertyModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty(RemovePropertyModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _apiResourceCommandService.RemoveProperty(model.ParentId, model.PropertyId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return NoContent();
        }

    }
}
