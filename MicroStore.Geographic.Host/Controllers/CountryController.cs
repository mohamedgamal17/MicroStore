using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.Geographic.Application.Countries;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using MicroStore.Geographic.Application.StateProvinces;
using System.Net;

namespace MicroStore.Geographic.Host.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountryController : MicroStoreApiController
    {
        private readonly ICountryApplicationService _countryApplicationService;

        private readonly IStateProvinceApplicationService _stateProvinceApplicationService;


        public CountryController(ICountryApplicationService countryApplicationService, IStateProvinceApplicationService stateProvinceApplicationService)
        {
            _countryApplicationService = countryApplicationService;
            _stateProvinceApplicationService = stateProvinceApplicationService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CountryListDto>))]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> ListCountries()
        {
            var result = await _countryApplicationService.ListAsync();

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(CountryDto))]
        [HttpGet]
        [Route("{countryId}")]
        public async Task<IActionResult> GetCountry(string countryId)
        {
            var result = await _countryApplicationService.GetAsync(countryId);

            return FromResult(result, HttpStatusCode.OK);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryDto))]
        [HttpGet]
        [Route("code/{countryCode}")]
        public async Task<IActionResult> GetCountryByCode(string countryCode)
        {
            var result = await _countryApplicationService.GetByCodeAsync(countryCode);

            return FromResult(result, HttpStatusCode.OK);
        }


        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CountryDto))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult > CreateCountry([FromBody] CountryModel model)
        {
            var result= await _countryApplicationService.CreateAsync(model);
            return FromResult(result, HttpStatusCode.Created);
        }


        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryDto))]
        [HttpPut]
        [Route("{countryId}")]
        public async Task<IActionResult> UpdateCountry(string countryId , [FromBody]  CountryModel model)
        {
            var result = await _countryApplicationService.UpdateAsync(countryId, model);
            return FromResult(result, HttpStatusCode.Created);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(CountryDto))]
        [HttpDelete]
        [Route("{countryId}")]
        public async Task<IActionResult> DeleteCountry(string countryId)
        {
            var result = await _countryApplicationService.DeleteAsync(countryId);
            return FromResult(result, HttpStatusCode.NoContent);
        }



        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StateProvinceDto>))]
        [HttpGet]
        [Route("{countryId}/states")]
        public async Task<IActionResult> ListCountryStateProvinces(string countryId)
        {
            var result = await _stateProvinceApplicationService.ListAsync(countryId);
            return FromResult(result, HttpStatusCode.OK);
        }


        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StateProvinceDto>))]
        [HttpGet]
        [Route("{countryId}/states/{stateId}")]
        public async Task<IActionResult> GetCountryStateProvince(string countryId , string stateId)
        {
            var result = await _stateProvinceApplicationService.GetAsync(countryId, stateId);

            return FromResult(result, HttpStatusCode.OK);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StateProvinceDto))]
        [HttpGet]
        [Route("code/{countryCode}/states/code/{stateCode}")]
        public async Task<IActionResult> GetCountryStateProvinceByCode(string countryCode, string stateCode)
        {
            var result = await _stateProvinceApplicationService.GetByCodeAsync(countryCode, stateCode);

            return FromResult(result, HttpStatusCode.OK);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StateProvinceDto))]
        [HttpPost]
        [Route("{countryId}/states")]
        public async Task<IActionResult> CreateStateProvince(string countryId , [FromBody] StateProvinceModel model)
        {
            var result = await _stateProvinceApplicationService.CreateAsync(countryId, model);

            return FromResult(result, HttpStatusCode.Created);
        }

        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(StateProvinceDto))]
        [HttpPut]
        [Route("{countryId}/states/{stateId}")]
        public async Task<IActionResult> UpdateStateProvince(string countryId , string stateId ,[FromBody] StateProvinceModel model)
        {
            var result = await _stateProvinceApplicationService.UpdateAsync(countryId, stateId,model);

            return FromResult(result,HttpStatusCode.OK);

        }

        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(StateProvinceDto))]
        [HttpDelete]
        [Route("{countryId}/states/{stateId}")]
        public async Task<IActionResult> UpdateStateProvince(string countryId, string stateId)
        {
            var result = await _stateProvinceApplicationService.DeleteAsync(countryId, stateId);

            return FromResult(result, HttpStatusCode.NoContent);

        }

    }
}
