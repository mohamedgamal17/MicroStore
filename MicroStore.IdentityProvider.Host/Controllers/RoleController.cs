using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Host.Models;
using MicroStore.IdentityProvider.Identity.Application.Roles;

namespace MicroStore.IdentityProvider.Host.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController : MicroStoreApiController
    {

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetRoleList()
        {
            var query = new GetRoleListQuery { };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("name/{roleName}")]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            var query = new GetRoleWithNameQuery { Name = roleName };

            var result= await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{roleId}")]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var query = new GetRoleWithIdQuery { Id = roleId };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateRole([FromBody] RoleModel model)
        {
            var command = ObjectMapper.Map<RoleModel, CreateRoleCommand>(model);

            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPut]
        [Route("{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] RoleModel model)
        {
            var command = ObjectMapper.Map<RoleModel,UpdateRoleCommand>(model);

            command.RoleId = roleId;

            var result = await Send(command);

            return FromResult(result);
        }


    }
}
