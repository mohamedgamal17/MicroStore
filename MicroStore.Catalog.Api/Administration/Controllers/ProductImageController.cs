//using Microsoft.AspNetCore.Mvc;
//using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
//using MicroStore.Catalog.Api.Administration.Models.Products;
//using MicroStore.Catalog.Application.Abstractions.Common.Models;
//using MicroStore.Catalog.Application.Abstractions.Products.Commands;
//using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
//namespace MicroStore.Catalog.Api.Administration.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductImageController : ControllerBase
//    {
//        private readonly ILocalMessageBus _localMessageBus;

//        public ProductImageController(ILocalMessageBus localMessageBus)
//        {
//            _localMessageBus = localMessageBus;
//        }

//        [Route("{productId}")]
//        [HttpPost]
//        public async Task<ProductDto> Post(Guid productId,[FromBody] AssignProductImageModel model)
//        {
//            var command = new AssignProductImageCommand
//            {

//                ProductId = productId,
//                ImageModel = new ImageModel
//                {
//                    FileName = model.Image.FileName,
//                    Type = model.Image.FileName.Split(".")[1],
//                    Data = model.Image.GetAllBytes(),
//                },
//                DisplayOrder = model.DisplayOrder
//            };

//            var result = await _localMessageBus.Send(command);

//            return result;
//        }

//        [Route("{productId}/{productImageId}")]
//        [HttpPut]
//        public async Task<ProductDto> Put(Guid productId , Guid productImageId , [FromBody] UpdateProductImageModel model)
//        {
//            var command = new UpdateProductImageCommand
//            {
//                ProductId = productId,
//                ProductImageId = productImageId,
//                DisplayOrder = model.DisplayOrder
//            };

//            var result = await _localMessageBus.Send(command);

//            return result;
//        }

//        [Route("{productId}/{productImageId}")]
//        [HttpDelete]
//        public async Task<ProductDto> Delete(Guid productId, Guid productImageId)
//        {
//            var command = new RemoveProductImageCommand
//            {
//                ProductId = productId,
//                ProductImageId = productImageId
//            };

//            var result = await _localMessageBus.Send(command);

//            return result;
//        }
//    }
//}
