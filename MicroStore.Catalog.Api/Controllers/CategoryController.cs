using Microsoft.AspNetCore.Mvc;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using Volo.Abp;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Api.Models.Categories;
using MicroStore.Catalog.Application.Abstractions.Categories.Queries;

namespace MicroStore.Catalog.Api.Controllers
{
    [RemoteService(Name = "Categories")]
    [Area("Administration")]
    [Route("api/categories")]
    public class CategoryController : MicroStoreApiController
    {


        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(Envelope<List<CategoryListDto>>)))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetCatalogCategoryList()
        {
            var request = new GetCategoryListQuery();

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Post(CreateCategoryModel model)
        {
            CreateCategoryCommand comand = new CreateCategoryCommand
            {
                Name = model.Name,
                Description = model.Description
            };

            var result = await Send(comand);

            return FromResult(result);
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCategoryModel model)
        {
            UpdateCategoryCommand command = new UpdateCategoryCommand
            {
                CategoryId = id,
                Name = model.Name,
                Description = model.Description
            };

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
