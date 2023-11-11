using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Settings;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;
using System.Data;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]

    public class SettingsController : AdministrationController
    {
        private readonly ShipmentSettingsService _shipmentSettingsService;

        private readonly ShippingSystemService _shippingSystemService;

        private readonly CountryService _countryService;
        public SettingsController(ShipmentSettingsService shipmentSettingsService, ShippingSystemService shippingSystemService, CountryService countryService)
        {
            _shipmentSettingsService = shipmentSettingsService;
            _shippingSystemService = shippingSystemService;
            _countryService = countryService;
        }

        public async Task<IActionResult> Shipping()
        {
            var settings = await _shipmentSettingsService.GetAsync();

            var model = ObjectMapper.Map<ShipmentSettings, ShipmentSettingsModel>(settings);

            await PrepareShippingSetingsSelectionLists(model.DefaultShippingSystem, model.Location?.CountryCode, model.Location?.StateProvince);


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Shipping(ShipmentSettingsModel model)
        {
            if (!ModelState.IsValid)
            {

                await PrepareShippingSetingsSelectionLists(model.DefaultShippingSystem, model.Location?.CountryCode, model.Location?.StateProvince);

                return View(model);
            }

            try
            {
                var request = ObjectMapper.Map<ShipmentSettingsModel, ShipmentSettingsRequestOptions>(model);

                var reponse = await _shipmentSettingsService.UpdateAsync(request);

                NotificationManager.Success("Shipping settings has been updated successfully!");

                await PrepareShippingSetingsSelectionLists(model.DefaultShippingSystem, model.Location?.CountryCode, model.Location?.StateProvince);

                return View(model);

            }catch(MicroStoreClientException ex) when(ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                NotificationManager.Error(ex.Erorr.Title);

                ex.Erorr.MapToModelState(ModelState);

                await PrepareShippingSetingsSelectionLists(model.DefaultShippingSystem, model.Location?.CountryCode, model.Location?.StateProvince);

                return View(ModelState);
            }
         
        }


        private async Task PrepareShippingSetingsSelectionLists(string? shippingSystem = null,string? countryCode = null , string? stateProvince = null)
        {
            var systems = await _shippingSystemService.ListAsync();

            var countries = await _countryService.ListAsync();

            var country = countryCode != null ? await _countryService.GetByCodeAsync(countryCode) : new Country();


            ViewBag.ShippingSystems = systems.Select(x => new SelectListItem
            {
                Text = x.DisplayName,
                Value = x.Name,
                Selected = x.Name == shippingSystem
            }).ToList();

            ViewBag.Countries = countries.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.TwoLetterIsoCode,
                Selected = x.TwoLetterIsoCode == countryCode
            }).ToList();

            ViewBag.States = country.StateProvinces?.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Abbreviation,
                Selected = x.Abbreviation == stateProvince
            }).ToList();
        }
    }
}
