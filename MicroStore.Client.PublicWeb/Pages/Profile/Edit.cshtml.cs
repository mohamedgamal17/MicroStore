using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using System.Net;
using Volo.Abp.BlobStoring;

namespace MicroStore.Client.PublicWeb.Pages.Profile
{
    [Authorize]
    [CheckProfilePageCompletedFilter]
    public class EditModel : PageModel
    {
        [BindProperty]
        public UserProfileModel Profile { get; set; }

        public User UserProfile { get; set; }

        public List<Country> Countries { get; set; }

        private readonly UserProfileService _userProfileService;
        private readonly CountryService _countryService;
        private readonly IBlobContainer<MultiMediaBlobContainer> _blobContainer;
        private readonly UINotificationManager _notificationManager;
        private readonly ILogger<EditModel> _logger;

        public EditModel(UserProfileService userProfileService, CountryService countryService, IBlobContainer<MultiMediaBlobContainer> blobContainer, UINotificationManager notificationManager, ILogger<EditModel> logger)
        {
            _userProfileService = userProfileService;
            _countryService = countryService;
            _blobContainer = blobContainer;
            _notificationManager = notificationManager;
            _logger = logger;
        }



        public async Task<IActionResult> OnGetAsync()
        {
            UserProfile = (User)HttpContext.Items[HttpContextSharedItemsConsts.UserProfile]!;

            Profile = PrepareUserProfileModel(UserProfile);

            await PrepareCountries();

            return Page();
        }


        public  async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                UserProfile = (User)HttpContext.Items[HttpContextSharedItemsConsts.UserProfile]!;

                await PrepareCountries();

                return Page();
            }

            var requestOptions = new ProfileRequestOptions
            {
                FirstName = Profile.FirstName,
                LastName = Profile.LastName,
                BirthDate = Profile.BirthDate,
                Phone = Profile.Phone,
                Gender = Profile.Gender,
                Avatar = await GetUserImageLink(Profile.Avatar),
            };

            try
            {
                await _userProfileService.UpdateAsync(requestOptions);

                return RedirectToPage("/Profile/Index");
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                UserProfile = (User)HttpContext.Items[HttpContextSharedItemsConsts.UserProfile]!;

                await PrepareCountries();

                _notificationManager.Error(ex.Error.Title);

                return Page();
            }
        }

        public async Task PrepareCountries()
        {
            var response = await _countryService.ListAsync();

            Countries = response;
        }


        public async Task<string?> GetUserImageLink(IFormFile? formFile)
        {
            if (formFile == null) return null;

            string imageName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(formFile.FileName));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                await _blobContainer.SaveAsync(imageName, memoryStream.ToArray());
            }

            return HttpContext.GenerateFileLink(imageName);
        }


        private UserProfileModel PrepareUserProfileModel(User user)
        {
            return new UserProfileModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Gender = user.Gender,

                BirthDate = user.BirthDate
            };
        }
    }
}
