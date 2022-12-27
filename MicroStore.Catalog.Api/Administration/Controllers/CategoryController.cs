using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp;
using MicroStore.Catalog.Api.Administration.Models.Categories;
using MicroStore.Catalog.Api.Administration.Models;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;

namespace MicroStore.Catalog.Api.Administration.Controllers
{
    [RemoteService(Name = "Category")]
    [Area("Administration")]
    [Route("api/[Area]/[Controller]")]
    public class CategoryController : MicroStoreApiController
    {

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
