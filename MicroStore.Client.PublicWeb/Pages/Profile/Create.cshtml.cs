using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class CreateModel : PageModel
    {
        [BindProperty]
        public UserProfileModel Profile { get; set; }

        [BindProperty]
        public AddressModel Address { get; set; }

        [BindProperty]
        public string? ReturnUrl { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem>? StateProvinces { get; set; }

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
            var  userProfile = context.HttpContext.Items[HttpContextSharedItemsConsts.UserProfile] as User;

            if (userProfile != null)
            {
                uINotificationManager.Error("Your profile is already created!");

                context.Result = RedirectToPage("/Profile/Index");
            }

            await next();

        }

        public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;

            await PreapreGeographicSelectedLists();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PreapreGeographicSelectedLists(Address.Country, Address.StateProvince);

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
                await _userProfileService.CreateAsync(requestOptions);

                uINotificationManager.Success("Your profile has been successfully updated!");

                if(ReturnUrl != null)
                {
                    return Redirect(ReturnUrl);
                }

                return RedirectToPage("Profile/Index");
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                await PreapreGeographicSelectedLists(Address.Country, Address.StateProvince);

                uINotificationManager.Error(ex.Error.Title);

                return Page();
            }
        }
        public async Task PreapreGeographicSelectedLists(string? countryCode = null, string? stateProvince = null)
        {
            var countriesResponse = await _countryService.ListAsync();

            Countries = countriesResponse
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.TwoLetterIsoCode,
                    Selected = countryCode == x.TwoLetterIsoCode
                }).ToList();

            if (countryCode != null)
            {
                var countryResposnse = await _countryService.GetByCodeAsync(countryCode);

                StateProvinces = countryResposnse.StateProvinces?
                     .Select(x => new SelectListItem
                     {
                         Text = x.Name,
                         Value = x.Abbreviation,
                         Selected = stateProvince == x.Abbreviation
                     }).ToList();

            }

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
