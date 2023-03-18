using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Identity.Application.Models;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using System.Net;

namespace MicroStore.IdentityProvider.Host.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController : MicroStoreApiController
    {
        private readonly IRoleCommandService _roleCommandService;

        private readonly IRoleQueryService _roleQueryService;

        public RoleController(IRoleCommandService roleCommandService, IRoleQueryService roleQueryService)
        {
            _roleCommandService = roleCommandService;
            _roleQueryService = roleQueryService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetRoleList()
        {
            var result = await _roleQueryService.ListAsync();

            return result.ToOk();
        }

        [HttpGet]
        [ActionName(nameof(GetRoleByName))]
        [Route("name/{roleName}")]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            var result = await _roleQueryService.GetByNameAsync(roleName);

            return result.ToOk();
        }

        [HttpGet]
        [ActionName(nameof(GetRoleById))]
        [Route("{roleId}")]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var result = await _roleQueryService.GetAsync(roleId);

            return result.ToOk();
        }

        [HttpPost]
        [ActionName(nameof(CreateRole))]
        [Route("")]
        public async Task<IActionResult> CreateRole([FromBody] RoleModel model)
        {
            var result = await _roleCommandService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(GetRoleById), new { roleId = result.Value?.Id });
        }

        [HttpPut]
        [Route("{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] RoleModel model)
        {
            var result = await _roleCommandService.UpdateAsync(roleId, model);

            return result.ToOk();
        }


    }
}
