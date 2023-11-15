using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using System.Net;

namespace MicroStore.Client.PublicWeb.Pages.Profile.Address
{
    [Authorize]
    [CheckProfilePageCompletedFilter]
    public class EditModel : PageModel
    {
        private readonly UserProfileService _userProfileService;

        private readonly CountryService _countryService;

        private readonly StateProvinceService _stateProvinceService;

        private readonly UINotificationManager _notificatioManager;

        private readonly UserAddressService _userAddressService;

        [BindProperty]
        public AddressModel Address { get; set; }

        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem>? StateProvinces { get; set; }
        public EditModel(UserProfileService userProfileService, CountryService countryService, UINotificationManager notificatioManager, StateProvinceService stateProvinceService, UserAddressService userAddressService)
        {
            _userProfileService = userProfileService;
            _countryService = countryService;
            _notificatioManager = notificatioManager;
            _stateProvinceService = stateProvinceService;
            _userAddressService = userAddressService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var address = await _userAddressService.GetAsync(id);

            Address = PreapreAddressModel(address);

            await PreapreGeographicSelectedLists(address.CountryCode, Address.StateProvince);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PreapreGeographicSelectedLists(Address.Country, Address.StateProvince);

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
                await _userAddressService.UpdateAsync(Address.Id, requestOptions);


                _notificatioManager.Success("address has been updated!");

                return RedirectToPage("/Profile/Index");
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                await PreapreGeographicSelectedLists(Address.Country, Address.StateProvince);

                _notificatioManager.Error(ex.Erorr.Title);

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
        private AddressModel PreapreAddressModel(MicroStore.ShoppingGateway.ClinetSdk.Common.Address address)
        {
            return new AddressModel
            {  
                Id = address.Id,
                Name = address.Name,
                Country = address.CountryCode,
                StateProvince = address.State,
                City = address.City,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PhoneNumber = address.Phone,
                PostalCode = address.PostalCode,
                ZipCode = address.Zip
            };
        }
    }
}
