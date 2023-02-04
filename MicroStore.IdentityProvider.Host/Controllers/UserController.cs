using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Host.Models;
using MicroStore.IdentityProvider.Identity.Application.Users;
namespace MicroStore.IdentityProvider.Host.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : MicroStoreApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUserList([FromQuery] PagingQueryParams @params)
        {
            var query = new GetUserListQuery { PageNumber =@params.PageNumber, PageSize = @params.PageSize };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var query = new GetUserByIdQuery { UserId = userId };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var query = new GetUserByEmailQuery { Email = email };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("user-name/{userName}")]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            var query = new GetUserByUserNameQuery { UserName = userName };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            var command = ObjectMapper.Map<UserModel, CreateUserCommand>(model);

            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody]  UserModel model)
        {
            var command = ObjectMapper.Map<UserModel,UpdateUserCommand>(model);

            command.UserId = userId;

            var result = await Send(command);

            return FromResult(result);
        } 
    }
}
