using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Categories;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Catalog.Application.Models.Categories;
using MicroStore.Catalog.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : MicroStoreApiController
    {
        private readonly ICategoryQueryService _categoryQueryService;

        private readonly ICategoryCommandService _categoryCommandService;

        public CategoryController(ICategoryQueryService categoryQueryService, ICategoryCommandService categoryCommandService)
        {
            _categoryQueryService = categoryQueryService;
            _categoryCommandService = categoryCommandService;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(List<CategoryDto>)))]
         public async Task<IActionResult> GetCatalogCategoryList([FromQuery] CategoryListQueryModel queryParams)
        {
            var result = await  _categoryQueryService
                .ListAsync(queryParams);

            return result.ToOk();
        }


   
        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(CategoryDto)))]
        public async Task<IActionResult> GetCatalogCategory(string id)
        {
            var result = await _categoryQueryService.GetAsync(id);

            return result.ToOk();
        }


        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDto))]
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]
        public async Task<IActionResult> Post([FromBody]CategoryModel model)
        {
            var validationResult = await ValidateModel(model);

            if(!validationResult.IsValid) 
            {
                return InvalideModelState();
            }

            var result = await _categoryCommandService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(GetCatalogCategory), routeValues : new { id = result.Value.Id });
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]
        public async Task<IActionResult> Put(string id, [FromBody] CategoryModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }
            var result = await _categoryCommandService.UpdateAsync(id,model);

            return result.ToOk();
        }


     
    }
}
