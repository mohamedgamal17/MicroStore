using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using System.Net;
using Volo.Abp.BlobStoring;

namespace MicroStore.Client.PublicWeb.Pages.Profile
{
    [Authorize]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public UserProfileModel Profile { get; set; }

        [BindProperty]
        public AddressModel Address { get; set; }
        public List<Country> Countries { get; set; } = new List<Country>();

        private readonly UserProfileService _userProfileService;

        private readonly UINotificationManager uINotificationManager;

        private readonly CountryService _countryService;

        private readonly IBlobContainer<MultiMediaBlobContainer> _blobContainer;
        public CreateModel(UserProfileService userProfileService, UINotificationManager uINotificationManager, CountryService countryService, IBlobContainer<MultiMediaBlobContainer> blobContainer)
        {
            _userProfileService = userProfileService;
            this.uINotificationManager = uINotificationManager;
            _countryService = countryService;
            _blobContainer = blobContainer;
        }

        public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            bool userHasProfile = await CheckIfUserHasProfile();

            if (userHasProfile)
            {
                uINotificationManager.Error("Your profile is already created!");

                context.Result = RedirectToPage("/Profile/Index");
            }

            await next();

        }

        public async Task<IActionResult> OnGetAsync()
        {
            await PrepareCountries();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
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

                Addresses = new List<AddressRequestOptions>
                {
                    new AddressRequestOptions
                    {
                        Name = Address.Name,
                        CountryCode = Address.Country,
                        State = Address.StateProvince,
                        City = Address.City,
                        AddressLine1 = Address.AddressLine1,
                        AddressLine2 = Address.AddressLine2,
                        PostalCode = Address.PostalCode,
                        Zip = Address.ZipCode,
                        Phone = Address.PhoneNumber
                    }
                }

            };

            try
            {
                await _userProfileService.CreateProfileAsync(requestOptions);

                uINotificationManager.Success("Your profile has been successfully updated!");

                return RedirectToPage("Profile/Index");
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                await PrepareCountries();

                uINotificationManager.Error(ex.Erorr.Title);

                return Page();
            }
        }

        private async Task<bool> CheckIfUserHasProfile()
        {
            try
            {
                var profile = await _userProfileService.GetProfileAsync();

                return true;
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
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

            string imageName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(Profile.Avatar.FileName));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await Profile.Avatar.CopyToAsync(memoryStream);

                await _blobContainer.SaveAsync(imageName, memoryStream.ToArray());
            }

            return HttpContext.GenerateFileLink(imageName);
        }
    }
}
