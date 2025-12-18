using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using MimeMapping;
using System.Net;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
namespace MicroStore.Client.PublicWeb.Pages.Profile
{
    [Authorize]
    [CheckProfilePageCompletedFilter]
    public class IndexModel : AbpPageModel
    {

        private readonly UserProfileService _userPorfileService;

        private readonly UINotificationManager _notificationManager;

        private readonly IObjectStorageProvider _objectStorageProvider;

        private readonly UserAddressService _userAddressService;

  
        public User UserProfile { get; set; }
        public UserProfileModel Profile { get; set; }

        public IndexModel(UserProfileService userPorfileService, UINotificationManager notificationManager, IObjectStorageProvider objectStorageProvider, UserAddressService userAddressService)
        {
            _userPorfileService = userPorfileService;
            _notificationManager = notificationManager;
            _objectStorageProvider = objectStorageProvider;
            _userAddressService = userAddressService;
        }
        public IActionResult OnGet()
        {
            UserProfile = (User)HttpContext.Items[HttpContextSharedItemsConsts.UserProfile]!;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            UserProfile = (User)HttpContext.Items[HttpContextSharedItemsConsts.UserProfile]!;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var requestOptions = new ProfileRequestOptions
            {
                FirstName = Profile.FirstName,
                LastName = Profile.LastName,
                BirthDate = Profile.BirthDate,
                Phone = Profile.Phone,
                Gender = Profile.Gender,
                Avatar = await GetUserImageLink(Profile.Avatar)
            };


            await _userPorfileService.UpdateAsync(requestOptions);

            _notificationManager.Success("Your profile has been successfully updated!");

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAddressAsync(RemoveAddressModel model)
        {
            try
            {
                await _userAddressService.DeleteAsync(model.AddressId);

                return NoContent();
            }
            catch(MicroStoreClientException ex) when(ex.StatusCode != HttpStatusCode.NotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

        }

        public async Task<string?> GetUserImageLink(IFormFile? formFile)
        {
            if (formFile == null) return null;

            string imageName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(formFile.FileName));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);

                var args = new S3ObjectSaveArgs
                {
                    Name = imageName,
                    Data = memoryStream,
                    ContentType = MimeUtility.GetMimeMapping(formFile.FileName)
                };

                await _objectStorageProvider.SaveAsync(args);
            }

            return await _objectStorageProvider.CalculatePublicReferenceUrl(imageName);
        }

    }
}
