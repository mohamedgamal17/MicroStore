using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using System.Net;

namespace MicroStore.Client.PublicWeb.Pages.Profile.Address
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public AddressModel Address { get; set; }
        public List<Country> Countries { get; set; }

        public List<StateProvince> StateProvinces { get; set; }

        private readonly UserProfileService _userProfileService;

        private readonly CountryService _countryService;

        private readonly UINotificationManager _uINotificationManager;

        private readonly UserAddressService _userAddressService;
        public CreateModel(UserProfileService userProfileService, CountryService countryService, UINotificationManager uINotificationManager, UserAddressService userAddressService)
        {
            _userProfileService = userProfileService;
            _countryService = countryService;
            _uINotificationManager = uINotificationManager;
            _userAddressService = userAddressService;
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

                await PreapreStates(Address.Country);

                return Page();
            }

            var requestOptions = new AddressRequestOptions
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
            };

            try
            {
                var address =  await _userAddressService.CreateAsync(requestOptions);

                _uINotificationManager.Success("New address has been created");

                return RedirectToPage("/Profile/Index");
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                await PrepareCountries();

                await PreapreStates(Address.Country);

                _uINotificationManager.Error(ex.Erorr.Title);

                return Page();
            }

     

        }

        public async Task PrepareCountries()
        {
            var response = await _countryService.ListAsync();

            Countries = response;
        }

        public async Task PreapreStates(string countryCode)
        {
            if(countryCode != null)
            {
                var country = await _countryService.GetByCodeAsync(countryCode);

                StateProvinces = country.StateProvinces;
            }

        }
    }
}
