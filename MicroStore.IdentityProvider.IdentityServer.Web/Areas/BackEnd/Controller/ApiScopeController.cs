using Microsoft.AspNetCore.Mvc;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiScopes;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Controller
{
    public class ApiScopeController : BackEndController
    {

        private readonly IApiScopeCommandService _apiScopeCommandService;

        private readonly IApiScopeQueryService _apiScopeQueryService;

        public ApiScopeController(IApiScopeCommandService apiScopeCommandService, IApiScopeQueryService apiScopeQueryService)
        {
            _apiScopeCommandService = apiScopeCommandService;
            _apiScopeQueryService = apiScopeQueryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ApiScopeSearchModel model)
        {
            var queryParams = new ApiScopeListQueryModel
            {
                Name = model.Name
            };

            var result = await _apiScopeQueryService.ListAsync(queryParams);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            var viewModel = new ApiScopeListViewModel
            {
                Data = result.Value,
                Draw = model.Draw
            };


            return Json(viewModel);

        }
        public IActionResult Create()
        {
            return View(new CreateOrEditApiScopeModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateOrEditApiScopeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var apiScopeModel = ObjectMapper.Map<CreateOrEditApiScopeModel, ApiScopeModel>(model);

            var result = await _apiScopeCommandService.CreateAsync(apiScopeModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result, model);
            }


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _apiScopeQueryService.GetAsync(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            var model = ObjectMapper.Map<ApiScopeDto, CreateOrEditApiScopeModel>(result.Value);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateOrEditApiScopeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var apiScopeModel = ObjectMapper.Map<CreateOrEditApiScopeModel, ApiScopeModel>(model);

            var result = await _apiScopeCommandService.UpdateAsync(id, apiScopeModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result, model);
            }

            return View(model);
        }



        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _apiScopeCommandService.DeleteAsync(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }


            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> ListApiScopeProperties(int id, ListModel model)
        {
            var result = await _apiScopeQueryService.ListProperties(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }


            var viewModel = new ApiScopePropertyListViewModel
            {
                Data = ObjectMapper.Map<List<ApiScopePropertyDto>, List<PropertyViewModel>>(result.Value),
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
            var result = await _apiScopeQueryService.GetProperty(parentId, propertyId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            return PartialView("_Edit.Property", new PropertyViewModel
            {
                ParentId = result.Value.ScopeId,
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

            var result = await _apiScopeCommandService.AddProperty(model.ParentId, propertyModel);

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

            var result = await _apiScopeCommandService.UpdateProperty(model.ParentId, model.PropertyId, propertyModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty([FromBody] RemovePropertyModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _apiScopeCommandService.RemoveProperty(model.ParentId, model.PropertyId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return NoContent();
        }
    }
}
