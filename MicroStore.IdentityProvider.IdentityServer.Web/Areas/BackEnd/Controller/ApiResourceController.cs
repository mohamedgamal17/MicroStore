using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Controller
{
    [Authorize]
    public class ApiResourceController : BackEndController
    {
        private readonly IApiResourceCommandService _apiResourceCommandService;

        private readonly IApiResourceQueryService _apiResourceQueryService;

        public ApiResourceController(IApiResourceCommandService apiResourceCommandService, IApiResourceQueryService apiResourceQueryService)
        {
            _apiResourceCommandService = apiResourceCommandService;
            _apiResourceQueryService = apiResourceQueryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ApiResourceSearchModel model)
        {
            var queryParams = new ApiResourceListQueryModel 
            { 
                Length = model.PageSize,
                Skip = model.Skip ,
                Name = model.Name
            };

            var result = await _apiResourceQueryService.ListAsync(queryParams);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            var viewModel = new ApiResourceListViewModel
            {
                Start = result.Value.Skip,
                Length = result.Value.Lenght,
                Draw = model.Draw,
                RecordsTotal = result.Value.TotalCount,
                Data = result.Value.Items
            };

            return Json(viewModel);
        }

        public IActionResult Create()
        {
            return View(new CreateOrEditApiResourceModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrEditApiResourceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var apiResourceModel = ObjectMapper.Map<CreateOrEditApiResourceModel, ApiResourceModel>(model);

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

            var model = ObjectMapper.Map<ApiResourceDto, CreateOrEditApiResourceModel>(result.Value);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateOrEditApiResourceModel model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            var apiResourceModel = ObjectMapper.Map<CreateOrEditApiResourceModel, ApiResourceModel>(model);

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
        public async Task<IActionResult> ListApiResourceSecrets(int id, ListModel model)
        {
            var result = await _apiResourceQueryService.ListApiResourceSecrets(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            var viewModel = new ApiResourceSecretListViewModel
            {
                Data = result.Value,
                Draw = model.Draw
            };

            return Json(viewModel);
        }



        public IActionResult CreateApiResourceSecretModel(int apiResourceId)
        {
            return PartialView("_Create.ApiResourceSecretModal", new CreateApiResourceSecretModel { ApiResourceId = apiResourceId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateApiResourceSecret(CreateApiResourceSecretModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);

            }

            var apiResourceSecretModel = ObjectMapper.Map<CreateApiResourceSecretModel, SecretModel>(model);

            var result = await _apiResourceCommandService.AddSecret(model.ApiResourceId, apiResourceSecretModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiResourceSecret([FromBody] RemoveApiResourceSecretModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await _apiResourceCommandService.RemoveSecret(model.ApiResourceId, model.SecretId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return NoContent();
        }



        [HttpPost]
        public async Task<IActionResult> ListApiResourceProperties(int id, ListModel model)
        {
            var result = await _apiResourceQueryService.ListProperties(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            var viewModel = new ApiResourcePropertyListViewModel
            {
                Data = ObjectMapper.Map<List<ApiResourcePropertyDto>, List<PropertyViewModel>>(result.Value),
                Draw = model.Draw
            };


            return Json(viewModel);
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
        public async Task<IActionResult> DeleteProperty([FromBody]RemovePropertyModel model)
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
