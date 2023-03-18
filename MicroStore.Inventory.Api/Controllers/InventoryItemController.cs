using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Application.Products;
namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
   // [Authorize]
    [Route("api/inventory/products")]
    public class InventoryItemController : MicroStoreApiController
    {
        private readonly IProductCommandService _productCommandService;

        private readonly IProductQueryService _productQueryService;

        public InventoryItemController(IProductCommandService productCommandService, IProductQueryService productQueryService)
        {
            _productCommandService = productCommandService;
            _productQueryService = productQueryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(PagedResult<ProductDto>))]

        public async Task<IActionResult> RetriveProductList([FromQuery] PagingParamsQueryString @params)
        {
            var query = new PagingQueryParams
            {
                PageNumber = @params.PageNumber,
                PageSize = @params.PageSize,
            };

            var result = await _productQueryService.ListAsync(query);

            return result.ToOk();

        }


        [HttpGet]
        [Route("sku/{sku}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<IActionResult> RetriveProductWithSku(string sku)
        {
  
            var result = await _productQueryService.GetBySkyAsync(sku);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
 
        public async Task<IActionResult> RetriveProductWithExternalId(string productId)
        {
            var result = await _productQueryService.GetAsync(productId);

            return result.ToOk();
        }

        [HttpPost]
        [Route("adjustquantity/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]


        public async Task<IActionResult> AdjustProductInventory(string productId, [FromBody] AdjustProductInventoryModel model)
        {

            var result = await _productCommandService.AdjustInventory(productId, model);

            return result.ToOk();
        }
    }
}
