using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Models.Catalog.Categories;
using MicroStore.Bff.Shopping.Services.Catalog;
namespace MicroStore.Bff.Shopping.Areas.Administration
{
    [ApiExplorerSettings(GroupName = "Administration")]
    [Route("api/administration/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        public async Task<List<Category>> ListAsync(string name = "", string sortBy = "", bool desc = false)
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


        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        public async Task<Category> CreateAsync(CategoryModel model)
        {
            var result = await _categoryService.CreateAsync(model);

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        public async Task<Category> UpdateAsync(string id,  CategoryModel model)
        {
            var result = await _categoryService.UpdateAsync(id, model);

            return result;
        }
    }
}
