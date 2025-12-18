using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.Profiling.Application.Dtos;
using MicroStore.Profiling.Application.Models;
using MicroStore.Profiling.Application.Security;
using MicroStore.Profiling.Application.Services;
namespace MicroStore.Profiling.Host.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    public class ProfileController : MicroStoreApiController
    {
        private readonly IProfileCommandService _profileCommandService;

        private readonly IProfileQueryService _profileQueryService;

        private readonly ILogger<ProfileController> _logger;
        public ProfileController(IProfileCommandService profileCommandService, IProfileQueryService profileQueryService, ILogger<ProfileController> logger)
        {
            _profileCommandService = profileCommandService;
            _profileQueryService = profileQueryService;
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ProfileDto>))]
        public async Task<IActionResult> ListAsync([FromQuery]PagingQueryParams queryParams)
        {
            var result = await _profileQueryService.ListAsync(queryParams);

            return result.ToOk();
        }

        [Route("{userId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(ProfileDto))]
        public async Task<IActionResult> GetAsync(string userId)
        {
            var result = await _profileQueryService.GetAsync(userId);

            return result.ToOk();
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProfileDto))]
        [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> CreateAsync([FromBody]CreateProfileModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _profileCommandService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(GetAsync), new { userId = result.Value?.UserId });
        }


        [Route("{userId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileDto))]
        [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> UpdateAsync(string userId,[FromBody]ProfileModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _profileCommandService.UpdateAsync(userId, model);

            return result.ToOk();

        }

        [Route("/{userId}/addresses")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddressDto>))]
        public async Task<IActionResult> ListAddressesAsync(string userId)
        {
            var result = await _profileQueryService.ListAddressesAsync(userId);

            return result.ToOk();
        }

        [Route("/{userId}/addresses/{addressId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(AddressDto))]
        public async Task<IActionResult> GetAddressesAsync(string userId, string addressId)
        {
            var result = await _profileQueryService.GetAddressAsync(userId,addressId);

            return result.ToOk();
        }


        [Route("/{userId}/addresses")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddressDto))]
        [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> CreateAddresssAsync(string userId , [FromBody] AddressModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _profileCommandService.CreateAddressAsync(userId, model);

            return result.ToCreatedAtAction(nameof(GetAddressesAsync), new { userId = userId, addressId = result.Value?.Id });
        }


        [Route("/{userId}/addresses/{addressId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
        [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> UpdateAddressAsync(string userId,string addressId ,[FromBody] AddressModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _profileCommandService.UpdateAddressAsync(userId,addressId ,model);

            return result.ToOk();
        }

        [Route("/{userId}/addresses/{addressId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> DeleteAddressAsync(string userId, string addressId)
        {
            var result = await _profileCommandService.RemoveAddressAsync(userId, addressId);

            return result.ToNoContent();

        }


    }
}
