//using Microsoft.AspNetCore.Mvc;
//using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
//using MicroStore.Catalog.Api.Administration.Models.Products;
//using MicroStore.Catalog.Application.Abstractions.Products.Commands;
//using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
//namespace MicroStore.Catalog.Api.Administration.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductCategoryController : ControllerBase
//    {
//        private readonly ILocalMessageBus _localMessageBus;

//        public ProductCategoryController(ILocalMessageBus localMessageBus)
//        {
//            _localMessageBus = localMessageBus;
//        }

//        [Route("{productId}")]
//        [HttpPost]
//        public async Task<ProductDto> Post(Guid productId, [FromBody] AssignProductCategoryModel model)
//        {
//            var command = new AssignProductCategoryCommand
//            {
//                ProductId = productId,
//                CategoryId = model.CategoryId,
//                IsFeatured = model.IsFeatured,
//            };

//            var result = await _localMessageBus.Send(command);

//            return result;
//        }

//        [Route("{productId/update/{categoryId}}")]
//        [HttpPut]
//        public async Task<ProductDto> Put(Guid productId, Guid categoryId, [FromBody] UpdateProductCategoryModel model) 
//        {
//            var command = new UpdateProductCategoryCommand
//            {
//                ProductId = productId,
//                CategoryId = categoryId,
//                IsFeatured = model.IsFeatured
//            };

//            var result = await _localMessageBus.Send(command);

//            return result;
//        }

//        [Route("{productId/delete/{categoryId}}")]
//        public async Task<ProductDto> Delete(Guid productId, Guid categoryId)
//        {
//            var command = new RemoveProductCategoryCommand
//            {
//                CategoryId = categoryId,
//                ProductId = productId
//            };

//            var result = await _localMessageBus.Send(command);

//            return result;
//        }
//    }
//}
