using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Identity.Application.Models;
using MicroStore.IdentityProvider.Identity.Application.Users;
using System.Net;
namespace MicroStore.IdentityProvider.Host.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : MicroStoreApiController
    {
        private readonly IUserCommandService _userCommandService;
        private readonly IUserQueryService _userQueryService;

        public UserController(IUserCommandService userCommandService, IUserQueryService userQueryService)
        {
            _userCommandService = userCommandService;
            _userQueryService = userQueryService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUserList([FromQuery] PagingParamsQueryString @params)
        {
            var queryParams = new PagingQueryParams { PageNumber =@params.PageNumber, PageSize = @params.PageSize };

            var result = await _userQueryService.ListAsync(queryParams);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _userQueryService.GetAsync(userId);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _userQueryService.GetAsync(email);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("user-name/{userName}")]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            var result = await _userQueryService.GetByUserNameAsync(userName);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            var result = await _userCommandService.CreateUserAsync(model);

            return FromResultV2(result, HttpStatusCode.Created);
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody]  UserModel model)
        {
            var result = await _userCommandService.UpdateUserAsync(userId,model);

            return FromResultV2(result, HttpStatusCode.Created);
        } 
    }
}
