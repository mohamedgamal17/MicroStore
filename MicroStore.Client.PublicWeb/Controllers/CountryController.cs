using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using System.Net;

namespace MicroStore.Client.PublicWeb.Controllers
{
    [Route("api/country")]
    public class CountryController : Controller
    {
        private readonly CountryService _countryService;

        public CountryController(CountryService countryService)
        {
            _countryService = countryService;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> ListCountries()
        {
            var countries = await _countryService.ListAsync();

            return Json(countries);
        }

        [HttpGet]
        [Route("{countryId}")]
        public async Task<IActionResult> GetCountry(string countryId)
        {
            try
            {
                var country = await _countryService.GetAsync(countryId);

                return Json(country);

            }
            catch(MicroStoreClientException ex)when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
        
        }

        [HttpGet]
        [Route("code/{countryCode}")]
        public async Task<IActionResult> GetCountryByIsoCode(string countryCode)
        {
            try
            {
                var country = await _countryService.GetByCodeAsync(countryCode);

                return Json(country);

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
        }
    }
}
