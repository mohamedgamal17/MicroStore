﻿using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Identity.Application.Models;
using MicroStore.IdentityProvider.Identity.Application.Users;
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
            var queryParams = new PagingQueryParams { Skip =@params.Skip, Lenght = @params.Lenght };

            var result = await _userQueryService.ListAsync(queryParams);

            return result.ToOk();
        }

        [HttpGet]
        [ActionName(nameof(GetUserById))]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _userQueryService.GetAsync(userId);

            return result.ToOk();
        }

        [HttpGet]
        [ActionName(nameof(GetUserByEmail))]

        [Route("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _userQueryService.GetAsync(email);

            return result.ToOk();
        }

        [HttpGet]
        [ActionName(nameof(GetUserByName))]
        [Route("user-name/{userName}")]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            var result = await _userQueryService.GetByUserNameAsync(userName);

            return result.ToOk();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            var result = await _userCommandService.CreateUserAsync(model);

            return result.ToCreatedAtAction(nameof(GetUserById), new { userId = result.Value?.Id });
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody]  UserModel model)
        {
            var result = await _userCommandService.UpdateUserAsync(userId,model);

            return result.ToOk();
        } 
    }
}
