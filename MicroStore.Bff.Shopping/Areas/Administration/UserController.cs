using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Profiling;
using MicroStore.Bff.Shopping.Models.Profiling;
using MicroStore.Bff.Shopping.Models.Common;
using MicroStore.Bff.Shopping.Services.Profiling;

namespace MicroStore.Bff.Shopping.Areas.Administration
{
    [ApiExplorerSettings(GroupName = "Administration")]
    [Route("api/administration/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ProfilingService _profilingService;

        public UserController(ProfilingService profilingService)
        {
            _profilingService = profilingService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<UserProfile>))]
        public async Task<IActionResult> ListUsersAsync(int skip = 0, int length = 10)
        {
            var result = await _profilingService.ListAsync(skip, length);

            return Ok(result);
        }

        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfile))]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var result = await _profilingService.GetUserAsync(userId);

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserProfile))]
        public async Task<IActionResult> CreateUserProfileAsync(CreateUserProfileModel model)
        {
            var result = await _profilingService.CreateAsync(model.UserId,model);

            return StatusCode(StatusCodes.Status201Created,result);
        }


        [HttpPut]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfile))]
        public async Task<IActionResult> UpdateUserProfileAsync(CreateUserProfileModel model)
        {
            var result = await _profilingService.UpdateAsync(model.UserId, model);

            return Ok(result);
        }

        [HttpGet]
        [Route("{userId}/addresses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        public async Task<IActionResult> ListAddressesAsync(string userId)
        {
            var addresses = await _profilingService.ListUserAddressAsync(userId);

            return Ok(addresses);
        }


        [HttpGet]
        [Route("{userId}/addresses/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        public async Task<IActionResult> GetAddressAsync(string userId,string addressId)
        {
            var address = await _profilingService.GetUserAddressAsync(userId, addressId);

            return Ok(address);
        }

        [HttpPost]
        [Route("{userId}/addresses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        public async Task<IActionResult> CreateAddressAsync(string userId,AddressModel model)
        {

            var address = await _profilingService.CreateAddressAsync(userId, model);

            return StatusCode(StatusCodes.Status201Created, address);
        }

        [HttpPut]
        [Route("{userId}/addresses/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        public async Task<IActionResult> UpdateAddressAsync(string userId,string addressId, AddressModel model)
        {
            var address = await _profilingService.UpdateAddressAsync(userId, addressId, model);

            return Ok(address);
        }

        [HttpDelete]
        [Route("{userId}/addresses/{addressId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveAddressAsync(string userId ,string addressId)
        {
            await _profilingService.RemoveAddressAsync(userId, addressId);

            return NoContent();
        }

    }
}
