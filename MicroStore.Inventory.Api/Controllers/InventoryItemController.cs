using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Api.Models;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/inventory/products")]
    public class InventoryItemController : MicroStoreApiController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Envelope<PagedResult<ProductDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveProductList([FromQuery] PagingQueryParams @params)
        {
            var query = new GetProductListQuery
            {
                PageNumber = @params.PageNumber,
                PageSize = @params.PageSize,
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("external_product_id/{externalId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveProductWithExternalId(string externalId)
        {
            var query = new GetProductWithExternalIdQuery
            {
                ExternalProductId = externalId
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("sku/{sku}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveProductWithSku(string sku)
        {
            var query = new GetProductWithSkuQuery
            {
                Sku = sku
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveProductWithExternalId(Guid productId)
        {
            var query = new GetProductQuery
            {
                ProductId = productId
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpPost]
        [Route("adjustquantity/{productsku}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ProductAdjustedInventoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> AdjustProductInventory(string productsku, [FromBody] AdjustProductInventoryModel model)
        {
            var command = new AdjustProductInventoryCommand
            {
                Sku = productsku,
                Stock = model.AdjustedQuantity,
                Reason = model.Reason
            };

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
