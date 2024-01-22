using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.Geographic;
using MicroStore.Bff.Shopping.Models.Geographic;
using MicroStore.Bff.Shopping.Services.Geographic;
namespace MicroStore.Bff.Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {

        private readonly CountryService _countryService;

        private readonly StateProvinceService _stateProvinceService;
        public CountryController(CountryService countryService)
        {
            _countryService = countryService;
        }


        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Country>))]
        public async Task<List<Country>> ListAsync()
        {
            var result = await _countryService.ListAsync();

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Country))]
        public async Task<Country> GetByIdAsync(string id)
        {
            var result = await _countryService.GetById(id);

            return result;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Country))]
        public async Task<Country> CreateAsync(CountryModel model)
        {
            var result = await _countryService.CreateAsync(model);

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Country))]
        public async Task<Country> UpdateAsync(string id,CountryModel model)
        {
            var result = await _countryService.UpdateAsync(id, model);

            return result;
        }


        [HttpGet]
        [Route("{countryId}/stateprovinces")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StateProvince>))]
        public async Task<List<StateProvince>> ListStateProvincesAsync(string countryId)
        {
            var result = await _stateProvinceService.ListAsync(countryId);

            return result;
        }

        [HttpGet]
        [Route("{countryId}/stateprovinces/{stateProvinceId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StateProvince))]
        public async Task<StateProvince> GetStateProvincesAsync(string countryId, string stateProvinceId)
        {
            var result = await _stateProvinceService.GetAsync(countryId, stateProvinceId);

            return result;
        }

        [HttpGet]
        [Route("{countryCode}/stateprovinces/{abbrevation}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StateProvince))]
        public async Task<StateProvince> GetStateProvincesByCodeAsync(string countryCode, string abbrevation)
        {
            var result = await _stateProvinceService.GetAsync(countryCode, abbrevation);

            return result;
        }


        [HttpPost]
        [Route("{countryId}/stateprovinces")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StateProvince))]
        public async Task<StateProvince> CreateStateProvincesAsync(string countryId, StateProvinceModel model)
        {
            var result = await _stateProvinceService.CreateAsync(countryId, model);

            return result;
        }

        [HttpPut]
        [Route("{countryId}/stateprovinces/{stateProvinceId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StateProvince))]
        public async Task<StateProvince> CreateStateProvincesAsync(string countryId,string stateProvinceId, StateProvinceModel model)
        {
            var result = await _stateProvinceService.UpdateAsync(countryId, stateProvinceId, model);

            return result;
        }
    }
}
