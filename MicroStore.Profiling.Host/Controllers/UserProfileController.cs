﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Profiling.Application.Dtos;
using MicroStore.Profiling.Application.Models;
using MicroStore.Profiling.Application.Security;
using MicroStore.Profiling.Application.Services;
namespace MicroStore.Profiling.Host.Controllers
{
    [ApiController]
    [Route("api/user/profile")]
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
    public class UserProfileController : MicroStoreApiController
    {
        private readonly IProfileQueryService _profileQueryService;

        private readonly IProfileCommandService _profileCommandService;
        public UserProfileController(IProfileQueryService profileQueryService, IProfileCommandService profileCommandService)
        {
            _profileQueryService = profileQueryService;
            _profileCommandService = profileCommandService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileDto))]
        public async Task<IActionResult> GetAsync()
        {
            var userId = CurrentUser.Id!.ToString()!;

            var result = await _profileQueryService.GetAsync(userId);

            return result.ToOk();
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProfileDto))]
        public async Task<IActionResult> CreateAsync(ProfileModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var userId = CurrentUser.Id!.ToString()!;

            var createModel = ObjectMapper.Map<ProfileModel, CreateProfileModel>(model);

            createModel.UserId = userId;

            var result = await _profileCommandService.CreateAsync(createModel);

            return result.ToCreatedAtAction(nameof(GetAsync),routeValues: null);
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileDto))]
        public async Task<IActionResult> UpdateAsync(ProfileModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var userId = CurrentUser.Id!.ToString()!;

            var result = await _profileCommandService.UpdateAsync(userId,model);

            return result.ToOk();
        }

        [HttpGet]
        [Route("/addresses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddressDto>))]
        public async Task<IActionResult> ListAddressesAsync()
        {
            var userId = CurrentUser.Id!.ToString()!;

            var result = await _profileQueryService.ListAddressesAsync(userId);

            return result.ToOk();
        }

        [HttpGet]
        [Route("/addresses/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]

        public async Task<IActionResult> GetAddressAsync(string addressId)
        {
            var userId = CurrentUser.Id!.ToString()!;

            var result = await _profileQueryService.GetAddressAsync(userId,addressId);

            return result.ToOk();
        }

        [HttpPost]
        [Route("/addresses")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddressDto))]
        public async Task<IActionResult> CreateAddressAsync(AddressModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var userId = CurrentUser.Id!.ToString()!;

            var result = await _profileCommandService.CreateAddressAsync(userId, model);

            return result.ToCreatedAtAction(nameof(GetAddressAsync),new {addressId = result.Value?.Id});
        }

        [HttpPut]
        [Route("/addresses/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
        public async Task<IActionResult> UpdateAddressAsync(string addressId, AddressModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var userId = CurrentUser.Id!.ToString()!;

            var result = await _profileCommandService.UpdateAddressAsync(userId, addressId, model);

            return result.ToOk();

        }

        [HttpDelete]
        [Route("/addresses/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
        public async Task<IActionResult> RemoveAddressAsync(string addressId)
        {
            var userId = CurrentUser.Id!.ToString()!;

            var result = await _profileCommandService.RemoveAddressAsync(userId, addressId);

            return result.ToNoContent();

        }



    }
}