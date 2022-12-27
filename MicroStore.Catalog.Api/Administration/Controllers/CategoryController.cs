//using Microsoft.AspNetCore.Mvc;
//using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
//using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
//using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
//using Volo.Abp.AspNetCore.Mvc;
//using Volo.Abp;
//using MicroStore.Catalog.Api.Administration.Models.Categories;
//using MicroStore.Catalog.Api.Administration.Models;

//namespace MicroStore.Catalog.Api.Administration.Controllers
//{
//    [RemoteService(Name = "Category")]
//    [Area("Administration")]
//    [Route("api/[Area]/[Controller]")]
//    public class CategoryController : AbpControllerBase
//    {
//        private readonly ILocalMessageBus _localMessageBus;

//        public CategoryController(ILocalMessageBus localMessageBus)
//        {
//            _localMessageBus = localMessageBus;
//        }


//        [Route("")]
//        [HttpPost]

//        public async Task<CategoryDto> Post(CreateCategoryModel model)
//        {
//            CreateCategoryCommand comand = new CreateCategoryCommand
//            {
//                Name = model.Name,
//                Description = model.Description
//            };

//            var result = await _localMessageBus.Send(comand);

//            return result;
//        }

//        [Route("{id}")]
//        [HttpPut]
//        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCategoryModel model)
//        {
//            UpdateCategoryCommand command = new UpdateCategoryCommand
//            {
//                CategoryId = id,
//                Name = model.Name,
//                Description = model.Description
//            };

//            var result = await _localMessageBus.Send(command);

//            return Ok();
//        }
//    }
//}
