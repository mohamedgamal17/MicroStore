using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Models.Roles;
namespace MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Controllers
{
    [Authorize]
    public class RoleController : BackEndController
    {

        private readonly IRoleQueryService _roleQueryService;

        private readonly IRoleCommandService _roleCommandService;

        public RoleController(IRoleQueryService roleQueryService, IRoleCommandService roleCommandService)
        {
            _roleQueryService = roleQueryService;
            _roleCommandService = roleCommandService;
        }

        public IActionResult Index()
        {
            return View(new RoleSearchModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(RoleSearchModel model)
        {
            var queryParams = new RoleListQueryModel
            {
                Name = model.Name
            };

            var result = await _roleQueryService.ListAsync(queryParams);

            var viewModel = new RoleListViewModel
            {
                Data = result.Value,
                Draw = model.Draw
            };

            return Json(viewModel);
        }

        public IActionResult Create()
        {
            return View(new RoleModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _roleCommandService.CreateAsync(model);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result, model);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var editModelResult = await PrepareEditRoleModel(id);

            if (editModelResult.IsFailure)
            {
                return HandleFailureResultWithView(editModelResult);
            }

            return View(editModelResult.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _roleCommandService.UpdateAsync(model.Id, model);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result, model);
            }


            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var  result = await _roleCommandService.RemoveAsync(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }
            return NoContent();
        }

        private async Task<Result<EditRoleModel>> PrepareEditRoleModel(string id)
        {
            var result = await _roleQueryService.GetAsync(id);

            if (result.IsFailure)
            {
                return new Result<EditRoleModel>(result.Exception);
            }

            return ObjectMapper.Map<IdentityRoleDto, EditRoleModel>(result.Value);
        }


    }
}
