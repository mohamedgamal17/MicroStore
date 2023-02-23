using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Categories;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using System.Net;
using MicroStore.Catalog.Application.Models;
using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/categories")]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(List<CategoryListDto>)))]
        public async Task<IActionResult> GetCatalogCategoryList(SortingParamsQueryString @params)
        {
            var result = await  _categoryQueryService
                .ListAsync(new SortingQueryParams { SortBy = @params.SortBy, Desc = @params.Desc});

            return FromResult(result,HttpStatusCode.OK);
        }


   
        [Route("category/{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(CategoryDto)))]
        public async Task<IActionResult> GetCatalogCategory(string id)
        {
            var result = await _categoryQueryService.GetAsync(id);

            return FromResult(result,HttpStatusCode.OK);
        }


        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDto))]
        public async Task<IActionResult> Post(CategoryModel model)
        {
            var result = await _categoryCommandService.CreateAsync(model);

            return FromResult(result,HttpStatusCode.Created);
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<IActionResult> Put(string id, [FromBody] CategoryModel model)
        {
            var result = await _categoryCommandService.UpdateAsync(id,model);

            return FromResult(result,HttpStatusCode.OK);
        }
    }
}
