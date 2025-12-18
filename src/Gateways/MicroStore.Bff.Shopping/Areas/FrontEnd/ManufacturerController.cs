using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Services.Catalog;
namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/manufacturers")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly ManufacturerService _manufacturerService;

        public ManufacturerController(ManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Manufacturer>))]
        public async Task<List<Manufacturer>> ListAsync(string name = "", string sortBy = "", bool desc = false)
        {
            var result = await _manufacturerService.ListAsync(name, sortBy, desc);

            return result;
        }


        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Manufacturer))]
        public async Task<Manufacturer> GetByIdAsync(string id)
        {
            var result = await _manufacturerService.GetAsync(id);

            return result;
        }
    }
}
