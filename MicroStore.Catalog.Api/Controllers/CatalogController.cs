//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
//using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
//using MicroStore.Catalog.Application.Abstractions.Categories.Queries;
//using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
//using MicroStore.Catalog.Application.Abstractions.Products.Queries;
//using System.Xml.Linq;
//using Volo.Abp.AspNetCore.Mvc;
//using Volo.Abp;

//namespace MicroStore.Catalog.Api.Controllers
//{
//    [RemoteService(Name = "Catalog")]
//    [Route("api/[controller]")]

//    public class CatalogController : AbpControllerBase
//    {

//        private readonly ILocalMessageBus _localMessageBus;
//        public CatalogController(ILocalMessageBus localMessageBus)
//        {
//            _localMessageBus = localMessageBus;
//        }

//        [Route("category")]
//        [HttpGet]
//        public async Task<List<CategoryDto>> GetCatalogCategoryList()
//        {
//            var request = new GetCategoryListQuery();
//            var result = await _localMessageBus.Send(request);
//            return result;
//        }


//        [Route("category/{id}")]
//        [HttpGet]
//        public async Task<CategoryDto> GetCatalogCategory(Guid id)
//        {
//            var request = new GetCategoryQuery()
//            {
//                Id = id
//            };


//            var result = await _localMessageBus.Send(request);

//            return result;
//        }

//        [Authorize]
//        [Route("product")]
//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<List<ProductDto>> GetCatalogProductList()
//        {
//            var request = new GetProductListQuery();

//            var result = await _localMessageBus.Send(request);

//            return result;
//        }


//        [Route("product/{id}")]
//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ProductDto> GetCatalogProduct(Guid id)
//        {
//            var query = new GetProductQuery() { Id = id };

//            var result = await _localMessageBus.Send(query);

//            return result;
//        }
//    }
//}
