using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Services.Catalog;
namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/categories")]
    [ApiController]
    public class CategoryController : Controller
    {

        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<Category>))]
        public async Task<List<Category>> ListAsync(string name = "" , string sortBy =  "", [FromQuery] bool desc = false )
        {
            var result = await _categoryService.ListAsync(name, sortBy, desc);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]

        public async Task<Category> GetByIdAsync(string id)
        {
            var result = await _categoryService.GetAsync(id);

            return result;
        }
    }
}
