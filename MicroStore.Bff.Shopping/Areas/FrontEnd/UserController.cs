using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.Profiling;
using MicroStore.Bff.Shopping.Infrastructure;
using MicroStore.Bff.Shopping.Models.Common;
using MicroStore.Bff.Shopping.Models.Profiling;
using MicroStore.Bff.Shopping.Services.Profiling;
namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ProfilingService _profileService;

        private readonly ApplicationCurrentUser _applicationCurrentUser;
        public UserController(ProfilingService profileService, ApplicationCurrentUser applicationCurrentUser)
        {
            _profileService = profileService;
            _applicationCurrentUser = applicationCurrentUser;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfile))]
        public async Task<IActionResult> GetUserAsync()
        {
            string userId = _applicationCurrentUser.UserId!;

            var userProfile = await _profileService.GetUserAsync(userId);

            return Ok(userProfile);
        }


        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserProfile))]
        public async Task<IActionResult> CreateProfileAsync(UserProfileModel model)
        {
            string userId = _applicationCurrentUser.UserId!;

            var userProfile = await _profileService.CreateAsync(userId, model);

            return StatusCode(StatusCodes.Status201Created, userProfile);
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserProfile))]
        public async Task<IActionResult> UpdateProfileAsync(UserProfileModel model)
        {
            string userId = _applicationCurrentUser.UserId!;

            var userProfile = await _profileService.UpdateAsync(userId, model);

            return Ok(userProfile);
        }

        [HttpGet]
        [Route("addresses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        public async Task<IActionResult> ListAddressesAsync()
        {
            string userId = _applicationCurrentUser.UserId!;

            var addresses = await _profileService.ListUserAddressAsync(userId);

            return Ok(addresses);
        }


        [HttpGet]
        [Route("addresses/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        public async Task<IActionResult> GetAddressAsync(string addressId)
        {
            string userId = _applicationCurrentUser.UserId!;

            var address = await _profileService.GetUserAddressAsync(userId, addressId);

            return Ok(address);
        }

        [HttpPost]
        [Route("addresses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        public async Task<IActionResult> CreateAddressAsync(AddressModel model)
        {
            string userId = _applicationCurrentUser.UserId!;

            var address = await _profileService.CreateAddressAsync(userId, model);

            return StatusCode(StatusCodes.Status201Created, address);
        }

        [HttpPut]
        [Route("addresses/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        public async Task<IActionResult> UpdateAddressAsync(string addressId, AddressModel model)
        {
            string userId = _applicationCurrentUser.UserId!;

            var address = await _profileService.UpdateAddressAsync(userId, addressId, model);

            return Ok(address);
        }

        [HttpDelete]
        [Route("addresses/{addressId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveAddressAsync(string addressId)
        {
            string userId = _applicationCurrentUser.UserId!;

            await _profileService.RemoveAddressAsync(userId, addressId);

            return NoContent();
        }
    }
}
