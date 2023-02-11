using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.BuildingBlocks.Paging.Params;
using Volo.Abp.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Categories;
using MicroStore.Catalog.Api.Models;
using MicroStore.BuildingBlocks.AspNetCore.Models;

namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/categories")]
    public class CategoryController : MicroStoreApiController
    {
    
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(Envelope<ListResultDto<CategoryListDto>>)))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCatalogCategoryList(SortingParamsQueryString @params)
        {
            var request = new GetCategoryListQuery
            {
                SortBy = @params.SortBy,
                Desc = @params.Desc,
            };

            var result = await Send(request);

            return FromResult(result);
        }


   
        [Route("category/{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(Envelope<CategoryDto>)))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCatalogCategory(Guid id)
        {
            var request = new GetCategoryQuery()
            {
                Id = id
            };

            var result = await Send(request);

            return FromResult(result);
        }


        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CategoryModel model)
        {
            CreateCategoryCommand comand = ObjectMapper.Map<CategoryModel, CreateCategoryCommand>(model);

            var result = await Send(comand);

            return FromResult(result);
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] CategoryModel model)
        {
            UpdateCategoryCommand command = ObjectMapper.Map<CategoryModel, UpdateCategoryCommand>(model);

            command.CategoryId = id;

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
