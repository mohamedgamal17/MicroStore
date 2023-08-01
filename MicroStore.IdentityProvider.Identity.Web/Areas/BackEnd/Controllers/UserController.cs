using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using MicroStore.IdentityProvider.Identity.Application.Users;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Models.Users;

namespace MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Controllers
{
    public class UserController : BackEndController
    {

        private readonly IUserCommandService _userCommandService;

        private readonly IUserQueryService _userQueryService;

        private readonly IRoleQueryService _roleQueryService;

        private readonly ILogger<UserController> _logger;
        public UserController(IUserCommandService userCommandService, IUserQueryService userQueryService, IRoleQueryService roleQueryService, ILogger<UserController> logger)
        {
            _userCommandService = userCommandService;
            _userQueryService = userQueryService;
            _roleQueryService = roleQueryService;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View(new UserListModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserListModel model)
        {
            var pagingParams = new PagingQueryParams
            {
                Skip = model.Skip,
                Length = model.Length
            };

            var result = await _userQueryService.ListAsync(pagingParams);

            var pagedList = result.Value!;

            model.Data = pagedList.Items;

            model.Length = pagedList.Lenght;

            model.RecordsTotal = pagedList.TotalCount;

            return Json(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await PrepareRoleSelectedList();

            return View(new UserModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await PrepareRoleSelectedList();

                _logger.LogWarning("{ModelState}", ModelState);
                return View(model);
            }

            var result = await _userCommandService.CreateUserAsync(model);

            if (result.IsFailure)
            {
                _logger.LogException(result.Exception);
                return HandleFailureResultWithView(result, model);
            }


            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(string id)
        {
            var modelResult = await PrepareUserEditModel(id);

            if (modelResult.IsFailure)
            {
                return HandleFailureResultWithView(modelResult);
            }

            ViewBag.Roles = await PrepareRoleSelectedList(modelResult.Value.UserRoles);

            return View(modelResult.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await PrepareRoleSelectedList(model.UserRoles);

                return View(model);
            }

            var updateReult = await _userCommandService.UpdateUserAsync(model.Id, model);

            if (updateReult.IsFailure)
            {
                return HandleFailureResultWithView(updateReult, model);
            }

            return RedirectToAction("Index");
        }



        private async Task<Result<EditUserModel>> PrepareUserEditModel(string id)
        {

            var result = await _userQueryService.GetAsync(id);

            if (result.IsFailure)
            {
                return new Result<EditUserModel>(result.Exception);
            }


            return ObjectMapper.Map<IdentityUserDto, EditUserModel>(result.Value);
        }

        private async Task<List<SelectListItem>> PrepareRoleSelectedList(List<string> selectedRoles = null)
        {
            var result = await _roleQueryService.ListAsync(new RoleListQueryModel());

            var roles = result.Value!;

            List<SelectListItem> selectListItems = roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name,
                Selected = selectedRoles?.Contains(x.Name) ?? false
            }).ToList();

            return selectListItems;
        }



    }
}
